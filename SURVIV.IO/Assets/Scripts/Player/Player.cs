using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : Human
{
    [Header("Player Health")]
    public Image healthBar;

    [Header("Joysticks")]
    public Joystick movementJoystick;
    public Joystick directionJoystick;

    [Header("Player Components")]
    public Rigidbody2D playerRb;
    public Transform playerHand;

    [Header("Weapon Picked up")]
    private GameObject currEquippedWeaponGameObj = null;
    private Weapon currEquippedWeaponScript = null;

    [Header("Ammo Inventory")]
    [SerializeField] public int currPistolAmmo;
    [SerializeField] public int currRifleAmmo;
    [SerializeField] public int currShotgunAmmo;
    private const int pistolAmmoCap = 90;
    private const int rifleAmmoCap = 120;
    private const int shotgunAmmoCap = 60;

    private Vector2 move;
    private float threshold = 0.01f;
    private Vector2 lastDir = Vector2.up;

    private bool isFiring = false;
    private float rifleCooldown = 0.5f;
    private float rifleCooldownTimer = 0f;

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

        HandleFiring();
        rifleCooldownTimer -= Time.deltaTime;
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
        transform.position = new Vector3
            (Mathf.Clamp(transform.position.x, -102f, 93f),
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
        playerRb.MovePosition(playerRb.position + move * moveSpeed * Time.deltaTime);
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        int result;

        switch (type)
        {
            case AmmoType.PistolAmmo:

                result = currPistolAmmo + amount;
                if (result >= pistolAmmoCap)
                {
                    currPistolAmmo = pistolAmmoCap;
                }
                else
                {
                    currPistolAmmo += amount;
                }
                break;
            case AmmoType.RifleAmmo:
                result = currRifleAmmo + amount;

                if ((result + amount) > rifleAmmoCap)
                {
                    currRifleAmmo = rifleAmmoCap;
                }
                else
                {
                    currRifleAmmo += amount;
                }
                break;
            case AmmoType.ShotgunAmmo:
                result = currShotgunAmmo + amount;

                if ((result + amount) > shotgunAmmoCap)
                {
                    currShotgunAmmo = shotgunAmmoCap;
                }
                else
                {
                    currShotgunAmmo += amount;
                }
                break;
        }

        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        UIManager.Instance.UpdatePistolAmmoCount(currPistolAmmo);
        UIManager.Instance.UpdateRifleAmmoCount(currRifleAmmo);
        UIManager.Instance.UpdateShotgunAmmoCount(currShotgunAmmo);

        if (currEquippedWeaponScript != null)
        {
            UIManager.Instance.UpdateAmmoInInventory(GetAmmoCountForWeaponType(currEquippedWeaponScript.weaponType));

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Weapon weapon = other.GetComponent<Weapon>();

        if (weapon != null)
        {
            EquipWeapon(weapon.weaponPrefab, weapon.weaponType);
            Destroy(other.gameObject); // destroys gun on the ground

            LoadAmmoIntoClip(weapon.weaponType);
        }

        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet)
        {
            Destroy(other.gameObject);
            TakeDamage(bullet.damage);
            healthBar.fillAmount = health / 100f;
            Debug.Log("Player took damage " + bullet.damage);
        }
    }

    private void LoadAmmoIntoClip(WeaponType weaponType)
    {
        int ammoToLoadInClip = GetAmmoCountForWeaponType(weaponType);
        int clipCapacity = currEquippedWeaponScript.clipCapacity;

        if (ammoToLoadInClip > clipCapacity)
        {
            int remainingAmmo = ammoToLoadInClip - clipCapacity;
            ammoToLoadInClip -= remainingAmmo;
        }
        else if (ammoToLoadInClip < clipCapacity)
        {
            ammoToLoadInClip = ammoToLoadInClip;
        }
        else
        {
            ammoToLoadInClip = clipCapacity;
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
        //ReloadAutomatically();

        currEquippedWeaponGameObj = Instantiate(gunPrefab, playerHand.position, playerHand.rotation);
        currEquippedWeaponGameObj.transform.SetParent(playerHand);

        WeaponSwitch.Instance.AddWeaponInInventory(currEquippedWeaponGameObj);

        currEquippedWeaponScript = currEquippedWeaponGameObj.GetComponent<Weapon>();

        HandleGunComponents();
        ReloadAutomatically();

        UIManager.Instance.UpdateCurrentClip(currEquippedWeaponScript.currClip);
    }

    private void HandleGunComponents()
    {
        Collider2D gunCollider = currEquippedWeaponGameObj.GetComponent<Collider2D>();
        if (gunCollider != null)
        {
            gunCollider.enabled = false;
        }

        Rigidbody2D gunRigidbody = currEquippedWeaponGameObj.GetComponent<Rigidbody2D>();
        if (gunRigidbody != null)
        {
            gunRigidbody.isKinematic = true;
            gunRigidbody.linearVelocity = Vector2.zero;
            gunRigidbody.angularVelocity = 0f;
        }
    }

    public void HandleFiring()
    {
        if (currEquippedWeaponScript == null || !currEquippedWeaponGameObj.activeInHierarchy)
            return;

        if (!isFiring)
            return;

        if (currEquippedWeaponScript.currClip == 0) // from below, moved here 
        {
            ReloadAutomatically();
        }

        if (currEquippedWeaponScript.weaponType == WeaponType.AutomaticRifle)
        {
            Debug.Log("Firing rifle rn.");
            currEquippedWeaponScript.Fire();
            UIManager.Instance.UpdateCurrentClip(currEquippedWeaponScript.currClip);
            rifleCooldownTimer = rifleCooldown;
        }
        else
        {
            Debug.Log("Not firing rifle rn.");
            currEquippedWeaponScript.Fire();
            UIManager.Instance.UpdateCurrentClip(currEquippedWeaponScript.currClip);
            isFiring = false;
        }
    }

    public void HoldFireButton()
    {
        isFiring = true;
    }

    public void ReleaseFireButton()
    {
        isFiring = false;
    }

    private void ReloadAutomatically()
    {
        if (currEquippedWeaponScript == null || currEquippedWeaponScript.isReloading)
            return;

        int inventoryAmmo = GetAmmoCountForWeaponType(currEquippedWeaponScript.weaponType);

        currEquippedWeaponScript.StartReload(inventoryAmmo, (bulletsUsed) =>
        {
            if (bulletsUsed <= 0) 
                return;

            switch (currEquippedWeaponScript.weaponType)
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

            UpdateAmmoUI();
            UIManager.Instance.UpdateCurrentClip(currEquippedWeaponScript.currClip);
        });
    }

    public void UpdateEquippedWeapon(GameObject weaponObj)
    {
        currEquippedWeaponGameObj = weaponObj;
        currEquippedWeaponScript = weaponObj.GetComponent<Weapon>();

        UIManager.Instance.UpdateCurrentClip(currEquippedWeaponScript.currClip);
        UpdateAmmoUI();
    }
}
