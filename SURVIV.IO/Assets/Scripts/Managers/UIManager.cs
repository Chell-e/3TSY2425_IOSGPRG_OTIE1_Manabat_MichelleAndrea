using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Ammo Inventory UI")]
    public TextMeshProUGUI pistolAmmoCountUI;
    public TextMeshProUGUI rifleAmmoCountUI;
    public TextMeshProUGUI shotgunAmmoCountUI;

    [Header("Current Weapon's Ammo UI")]
    public TextMeshProUGUI ammoInInventoryUI;
    public TextMeshProUGUI currentClipUI;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UpdatePistolAmmoCount(0);
        UpdateRifleAmmoCount(0);
        UpdateShotgunAmmoCount(0);
    }

    public void UpdatePistolAmmoCount(int count)
    {
        pistolAmmoCountUI.text = count.ToString();
    }

    public void UpdateRifleAmmoCount(int count)
    {
        rifleAmmoCountUI.text = count.ToString();
    }

    public void UpdateShotgunAmmoCount(int count)
    {
        shotgunAmmoCountUI.text = count.ToString();
    }

    public void UpdateAmmoInInventory(int count)
    {
        ammoInInventoryUI.text = count.ToString();
    }

    public void UpdateCurrentClip(int count)
    {
        currentClipUI.text = count.ToString();
    }

}
