using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 originalPos;

    public void Start()
    {
        originalPos = transform.position;
        GameManager.Instance.mainCamera = this.gameObject;
    }

    void Update()
    {
        transform.Translate(Vector2.up * Player.Instance.MovementSpeed * Time.deltaTime);
    }

    public void Reset()
    {
        this.transform.position = originalPos;
    }
}
