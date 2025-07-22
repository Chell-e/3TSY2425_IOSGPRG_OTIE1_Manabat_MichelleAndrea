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
    
    [SerializeField] private int ammoClip;

    public int ammoAmount;

    private void Start()
    {
        switch(ammoType)
        {
            case AmmoType.PistolAmmo: ammoClip = 15; break;
            case AmmoType.RifleAmmo: ammoClip = 30; break;
            case AmmoType.ShotgunAmmo: ammoClip = 2; break;
            default: ammoClip = 0; break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        if (player != null)
        {
            UIManager.Instance.AddAmmo(ammoType, ammoClip);
            Destroy(this.gameObject);
        }
    }
}
