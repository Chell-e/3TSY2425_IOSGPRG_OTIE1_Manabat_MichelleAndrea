using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;

    public Rigidbody2D rb;

    public float speed;

    private Vector2 move;
    private float threshold = 0.01f;

    void Update()
    {
        if (transform.position.y >= 48)
        {
            transform.position = new Vector3(transform.position.x, 48, 0);
        }
        else if (transform.position.y <= -48)
        {
            transform.position = new Vector3(transform.position.x, -48, 0);
        }

        if (transform.position.x <= -102)
        {
            transform.position = new Vector3(-102, transform.position.y, 0);
        }
        else if (transform.position.x >= 93)
        {
            transform.position = new Vector3(93, transform.position.y, 0);
        }

        move.x = joystick.Horizontal;
        move.y = joystick.Vertical;

        if (Mathf.Abs(move.x) < threshold) move.x = 0f;
        if (Mathf.Abs(move.y) < threshold) move.y = 0f;

        move = new Vector2(move.x, move.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }
}
