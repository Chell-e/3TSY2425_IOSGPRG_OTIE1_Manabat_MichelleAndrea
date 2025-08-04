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
    public float fireRate;
    public float nextFire = 0f;
    public float reloadSpeed;

    [Header("Ammo Properties")]
    public AmmoType ammoType;
    public int clipCapacity;
    public int currClip;

    [Header("Bullet Properties")]
    [SerializeField] int damage;
    public GameObject bulletPrefab;
    public Transform barrel;

    public bool isReloading = false;

    public bool Fire()
    {
        if (Time.time >= nextFire && currClip > 0)
        {
            nextFire = Time.time + fireRate;
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    PistolFiringMode();
                    break;
                case WeaponType.AutomaticRifle:
                    RifleFiringMode();
                    break;
                case WeaponType.Shotgun:
                    ShotgunFiringMode();
                    break;
                default:
                    Debug.Log("No firing mode activated.");
                    break;
            }
            return true;
        }

        return false;
    }

    private void PistolFiringMode()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        currClip--;
    }

    private void RifleFiringMode()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.damage = damage;
        currClip--;
    }

    private void ShotgunFiringMode()
    {
        for (int i = 0; i < 8; i++)
        {
            float spread = Random.Range(-20f, 20f);
            Quaternion rotation = Quaternion.Euler(-5, -5, barrel.eulerAngles.z + spread);

            GameObject bullet = Instantiate(bulletPrefab, barrel.position, rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.damage = damage;
        }

        currClip--;
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

        return bulletsToReload;
    }

    public void StartReload(int inventoryAmmo, System.Action<int> onReloadComplete = null)
    {
        if (!isReloading && currClip < clipCapacity)
        {
            StartCoroutine(ReloadCoroutine(inventoryAmmo, onReloadComplete));
        }
    }
    
    private IEnumerator ReloadCoroutine(int inventoryAmmo, System.Action<int> onReloadComplete)
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadSpeed);

        int bulletsReloaded = Reload(inventoryAmmo);
        isReloading = false;

        onReloadComplete?.Invoke(bulletsReloaded);
    }

}
