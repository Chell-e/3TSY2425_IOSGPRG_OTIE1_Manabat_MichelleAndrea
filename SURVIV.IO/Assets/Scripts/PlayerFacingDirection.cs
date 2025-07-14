using UnityEngine;

public class PlayerFacingDirection : MonoBehaviour
{
    public Joystick joystick;

    private Vector2 lastDir = Vector2.up;
    private float threshold = 0.01f;

    void Update()
    {
        float hAxis = joystick.Horizontal;
        float vAxis = joystick.Vertical;

        Vector2 inputDir = new Vector2(hAxis, vAxis);

        if (inputDir.sqrMagnitude > threshold)
        {
            lastDir = inputDir.normalized;
        }

        float zAxis = Mathf.Atan2(lastDir.x, lastDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, - zAxis);
    }
}
