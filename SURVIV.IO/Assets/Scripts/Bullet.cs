using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    public float speed = 15f;

    private Rigidbody2D bulletRb;

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = transform.up * speed;
        }

        Invoke("DestroyBullet", 5f);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
