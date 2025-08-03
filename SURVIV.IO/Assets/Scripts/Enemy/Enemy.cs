using UnityEngine;

public class Enemy : Human
{
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


}
