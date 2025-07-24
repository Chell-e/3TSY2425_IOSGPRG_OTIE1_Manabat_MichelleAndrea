using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Ammo UI")]
    public TextMeshProUGUI pistolAmmoCountUI;
    public TextMeshProUGUI rifleAmmoCountUI;
    public TextMeshProUGUI shotgunAmmoCountUI;

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
}
