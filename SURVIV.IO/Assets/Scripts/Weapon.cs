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

    public GameObject weaponPrefab;

}
