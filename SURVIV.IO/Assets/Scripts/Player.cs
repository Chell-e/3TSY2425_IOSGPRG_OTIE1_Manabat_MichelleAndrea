using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("Joysticks")]
    public Joystick movementJoystick;
    public Joystick directionJoystick;

    [Header("Player Components")]
    public Rigidbody2D playerRb;
    public Transform playerHand;

    [SerializeField] private float speed;

    [Header("Weapon Picked up")]
    private GameObject currEquippedGun = null;
    private Weapon currWeapon = null;

    [Header("Inventory")]
    private int currPistolAmmo;
    private int currRifleAmmo;
    private int currShotgunAmmo;

    private Vector2 move;
    private float threshold = 0.01f;
    private Vector2 lastDir = Vector2.up;

    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currPistolAmmo = 0;
        currRifleAmmo = 0;
        currShotgunAmmo = 0;

        UpdateAmmoUI();

        UIManager.Instance.UpdateAmmoInInventory(0);
        UIManager.Instance.UpdateCurrentClip(0);
    }

    void Update()
    {
        ManagePlayerMovement();
        ManageFacingDirection();

        ApplyScreenBounds();

        if (currWeapon != null)
        {
            int ammoCount = GetAmmoCountForWeaponType(currWeapon.weaponType);
            UIManager.Instance.UpdateAmmoInInventory(ammoCount);

            if (currWeapon != null && currWeapon.currClip == 0 && GetAmmoCountForWeaponType(currWeapon.weaponType) > 0)
            {
                ReloadAutomatically();
            }
        }
    }

    private void ManagePlayerMovement()
    {
        move.x = movementJoystick.Horizontal;
        move.y = movementJoystick.Vertical;

        if (Mathf.Abs(move.x) < threshold) move.x = 0f;
        if (Mathf.Abs(move.y) < threshold) move.y = 0f;

        move = new Vector2(move.x, move.y);
    }

    private void ApplyScreenBounds()
    {
        transform.position = new Vector3(Mathf.Clamp(
            transform.position.x, -102f, 93f),
            Mathf.Clamp(transform.position.y, -48f, 48f),
            0f);
    }

    private void ManageFacingDirection()
    {
        float hAxis = directionJoystick.Horizontal;
        float vAxis = directionJoystick.Vertical;

        Vector2 inputDir = new Vector2(hAxis, vAxis);

        if (inputDir.sqrMagnitude > threshold)
        {
            lastDir = inputDir.normalized;
        }

        float zAxis = Mathf.Atan2(lastDir.x, lastDir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, -zAxis);
    }

    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + move * speed * Time.deltaTime);
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        switch (type)
        {
            case AmmoType.PistolAmmo:
                currPistolAmmo += amount;
                break;
            case AmmoType.RifleAmmo:
                currRifleAmmo += amount;
                break;
            case AmmoType.ShotgunAmmo:
                currShotgunAmmo += amount;
                break;
        }

        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        UIManager.Instance.UpdatePistolAmmoCount(currPistolAmmo);
        UIManager.Instance.UpdateRifleAmmoCount(currRifleAmmo);
        UIManager.Instance.UpdateShotgunAmmoCount(currShotgunAmmo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon != null)
        {
            DiscardWeapon();

            EquipWeapon(weapon.weaponPrefab, weapon.weaponType);
            Destroy(other.gameObject);
        }
    }

    private void DiscardWeapon()
    {
        if (currEquippedGun != null)
        {
            Destroy(currEquippedGun);
            currWeapon = null;
        }
    }

    public int GetAmmoCountForWeaponType(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Pistol:
                return currPistolAmmo;
            case WeaponType.AutomaticRifle:
                return currRifleAmmo;
            case WeaponType.Shotgun:
                return currShotgunAmmo;
            default:
                return 0;
        }
    }

    private void EquipWeapon(GameObject gunPrefab, WeaponType gunType)
    {
        currEquippedGun = Instantiate(gunPrefab, playerHand.position, playerHand.rotation);
        currEquippedGun.transform.SetParent(playerHand);

        currWeapon = currEquippedGun.GetComponent<Weapon>();
        if (currWeapon == null)
            return;

        HandleGunComponents();

        int ammoCount = GetAmmoCountForWeaponType(gunType);
        UIManager.Instance.UpdateAmmoInInventory(ammoCount);
        
        int clipSize = currWeapon.clipCapacity;
        currWeapon.currClip = clipSize;
        
        if (ammoCount > 0)
        {
            UIManager.Instance.UpdateCurrentClip(clipSize);
        }
    }

    private void HandleGunComponents()
    {
        Collider2D gunCollider = currEquippedGun.GetComponent<Collider2D>();
        if (gunCollider != null)
        {
            gunCollider.enabled = false;
        }

        Rigidbody2D gunRigidbody = currEquippedGun.GetComponent<Rigidbody2D>();
        if (gunRigidbody != null)
        {
            gunRigidbody.isKinematic = true;
            gunRigidbody.linearVelocity = Vector2.zero;
            gunRigidbody.angularVelocity = 0f;
        }
    }

    public void HandleFiring()
    {
        if (currWeapon == null)
            return;

        bool didFire = currWeapon.Fire();

        if (!didFire && currWeapon.currClip == 0)
        {
            ReloadAutomatically();
        }
    }

    private void ReloadAutomatically()
    {
        if (currWeapon == null) 
            return;

        TrackBulletsUsed();
        UpdateAmmoUI();
        UIManager.Instance.UpdateAmmoInInventory(GetAmmoCountForWeaponType(currWeapon.weaponType));
    }

    private void TrackBulletsUsed()
    {
        int inventoryAmmo = GetAmmoCountForWeaponType(currWeapon.weaponType);
        if (inventoryAmmo <= 0 || currWeapon.currClip >= currWeapon.clipCapacity)
            return;


        int bulletsUsed = currWeapon.Reload(inventoryAmmo);
        if (bulletsUsed <= 0)
            return;

        switch (currWeapon.weaponType)
        {
            case WeaponType.Pistol:
                currPistolAmmo -= bulletsUsed;
                break;
            case WeaponType.AutomaticRifle:
                currRifleAmmo -= bulletsUsed;
                break;
            case WeaponType.Shotgun:
                currShotgunAmmo -= bulletsUsed;
                break;
        }
    }
}
