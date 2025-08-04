using UnityEngine;

public class Human : MonoBehaviour
{
    // common things enemy and player have
    [SerializeField] protected float health;
    [SerializeField] protected float moveSpeed = 10f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
