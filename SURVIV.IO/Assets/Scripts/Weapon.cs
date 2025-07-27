using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Pistol,
    AutomaticRifle,
    Shotgun
}

public class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    public WeaponType weaponType;
    public GameObject weaponPrefab;
    public float fireRate = 2.16f;
    public float nextFire = 0f;

    [Header("Ammo Properties")]
    public AmmoType ammoType;
    public int clipCapacity = 15;
    public int currClip;

    [Header("Bullet Properties")] 
    public GameObject bulletPrefab;
    public Transform barrel;

    public bool Fire()
    {
        if (Time.time >= nextFire && currClip > 0)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, barrel.position, barrel.rotation);
            currClip--;

            UIManager.Instance.UpdateCurrentClip(currClip);
            
            return true; 
        }

        return false; 
    }


    public int Reload(int inventoryAmmo)
    {
        int bulletsNeeded = clipCapacity - currClip;
        int bulletsToReload = 0;

        if (inventoryAmmo >= bulletsNeeded)
        {
            bulletsToReload = bulletsNeeded;
        }
        else if (inventoryAmmo > 0)
        {
            bulletsToReload = inventoryAmmo;
        }

        currClip += bulletsToReload;
        UIManager.Instance.UpdateCurrentClip(currClip);

        return bulletsToReload; 
    }

}
