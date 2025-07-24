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

    private int currPistolAmmo;
    private int currRifleAmmo;
    private int currShotgunAmmo;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currPistolAmmo = 0;
        currRifleAmmo = 0;
        currShotgunAmmo = 0;
        UpdateAmmoUI();
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

    public void AddAmmo(AmmoType type, int amount)
    {
        switch (type)
        {
            case AmmoType.PistolAmmo:
                currPistolAmmo += amount;
                break;
            case AmmoType.RifleAmmo:
                currRifleAmmo += amount;
                break;
            case AmmoType.ShotgunAmmo:
                currShotgunAmmo += amount;
                break;
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        UIManager.Instance.UpdatePistolAmmoCount(currPistolAmmo);
        UIManager.Instance.UpdateRifleAmmoCount(currRifleAmmo);
        UIManager.Instance.UpdateShotgunAmmoCount(currShotgunAmmo);
    }
}
