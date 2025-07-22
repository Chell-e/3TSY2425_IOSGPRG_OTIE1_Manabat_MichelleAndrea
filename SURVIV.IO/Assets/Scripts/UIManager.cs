using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI pistolAmmoCountUI;
    public TextMeshProUGUI rifleAmmoCountUI;
    public TextMeshProUGUI shotgunAmmoCountUI;

    private int pistolAmmoCount = 0;
    private int rifleAmmoCount = 0;
    private int shotgunAmmoCount = 0;

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
        UpdatePistolAmmoCount();
        UpdateRifleAmmoCount();
        UpdateShotgunAmmoCount();
    }

    private void UpdatePistolAmmoCount()
    {
        pistolAmmoCountUI.text = pistolAmmoCount.ToString();
    }

    private void UpdateRifleAmmoCount()
    {
        rifleAmmoCountUI.text = rifleAmmoCount.ToString();
    }

    private void UpdateShotgunAmmoCount()
    {
        shotgunAmmoCountUI.text = shotgunAmmoCount.ToString();
    }

    public void AddAmmo(AmmoType type, int amount)
    {
        switch(type)
        {
            case AmmoType.PistolAmmo:
                pistolAmmoCount += amount;
                UpdatePistolAmmoCount();
                break;
            case AmmoType.RifleAmmo:
                rifleAmmoCount += amount;
                UpdateRifleAmmoCount();
                break;
            case AmmoType.ShotgunAmmo:
                shotgunAmmoCount += amount;
                UpdateShotgunAmmoCount();
                break;
        }
    }
}
