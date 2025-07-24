using UnityEngine;

public enum WeaponType
{
    Pistol,
    AutomaticRifle,
    Shotgun
}

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Player picked up " + this.name);
            Destroy(this.gameObject);
        }
    }
}
