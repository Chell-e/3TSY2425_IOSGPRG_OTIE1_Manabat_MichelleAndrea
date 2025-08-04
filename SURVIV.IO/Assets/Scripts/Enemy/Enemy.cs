using UnityEngine;
using UnityEngine.UI;

public class Enemy : Human
{
    [Header("Enemy Health")]
    public Image healthBar;

    [Header("Random Weapons Prefabs")]
    [SerializeField] GameObject[] weaponPrefabs;

    [Header("Weapon Equipped")]
    [SerializeField] GameObject currEquippedWeaponObj;

    [Header("Enemy Components")]
    [SerializeField] Transform enemyHand;

    private void Start()
    {
        SpawnWithWeapon();
    }

    private void SpawnWithWeapon()
    {
        GameObject spawnedWeapon = Instantiate(weaponPrefabs[Random.Range(0, weaponPrefabs.Length)], enemyHand.position, enemyHand.rotation);

        currEquippedWeaponObj = spawnedWeapon;
        currEquippedWeaponObj.transform.SetParent(enemyHand);

        DisableGunComponents();
    }

    private void DisableGunComponents()
    {
        Collider2D gunCollider = currEquippedWeaponObj.GetComponent<Collider2D>();
        if (gunCollider != null)
        {
            gunCollider.enabled = false;
        }

        Rigidbody2D gunRigidbody = currEquippedWeaponObj.GetComponent<Rigidbody2D>();
        if (gunRigidbody != null)
        {
            gunRigidbody.isKinematic = true;
            gunRigidbody.linearVelocity = Vector2.zero;
            gunRigidbody.angularVelocity = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        
        if (bullet)
        {
            Destroy(other.gameObject);
            TakeDamage(bullet.damage);
            healthBar.fillAmount = health / 100f;
            Debug.Log("Enemy took damage" + bullet.damage);
        }
    }

}
