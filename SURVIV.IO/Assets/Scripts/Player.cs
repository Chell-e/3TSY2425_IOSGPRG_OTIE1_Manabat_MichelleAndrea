using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 move;
    private float threshold = 0.01f;

    private Vector2 lastDir = Vector2.up;

    public Joystick movementJoystick;
    public Joystick directionJoystick;

    public Rigidbody2D playerRb;

    public float speed;

    //private void Start()
    //{
    //    GameManager.Instance.player = this.gameObject;
    //}

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManagePlayerMovement();
        ManageFacingDirection();

        ScreenBounds();
    }

    private void ManagePlayerMovement()
    {
        move.x = movementJoystick.Horizontal;
        move.y = movementJoystick.Vertical;

        if (Mathf.Abs(move.x) < threshold) move.x = 0f;
        if (Mathf.Abs(move.y) < threshold) move.y = 0f;

        move = new Vector2(move.x, move.y);
    }

    private void ScreenBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(
            transform.position.x, -102f, 93f),
            Mathf.Clamp(transform.position.y, -48f, 48f),
            0f);
    }

    private void ManageFacingDirection()
    {
        float hAxis = directionJoystick.Horizontal;
        float vAxis = directionJoystick.Vertical;

        Vector2 inputDir = new Vector2(hAxis, vAxis);

        if (inputDir.sqrMagnitude > threshold)
        {
            lastDir = inputDir.normalized;
        }

        float zAxis = Mathf.Atan2(lastDir.x, lastDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, -zAxis);
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + move * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) // colliding w obstacles
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            Debug.Log("Player collided with " + collision.gameObject.name);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collider) // colliding w ammo
    //{
    //    Ammo ammo = collider.GetComponent<Ammo>();
        
    //    if (ammo != null)
    //    {
    //        Debug.Log("Player picks up " + ammo.ammoType.ToString());
    //        Destroy(ammo.gameObject);
    //    }
    //}
}
