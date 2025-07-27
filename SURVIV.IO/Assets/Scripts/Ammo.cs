using UnityEngine;

public enum AmmoType
{
    PistolAmmo,
    RifleAmmo,
    ShotgunAmmo
}

public class Ammo : MonoBehaviour
{
    public AmmoType ammoType;

    [SerializeField] private int minAmmo;
    [SerializeField] private int maxAmmo;

    [SerializeField] private int ammoGiven;
    
    private void Start()
    {
        ammoGiven = Random.Range(minAmmo, maxAmmo + 1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            Player.Instance.AddAmmo(ammoType, ammoGiven);
            Destroy(this.gameObject);
        }
    }
}
