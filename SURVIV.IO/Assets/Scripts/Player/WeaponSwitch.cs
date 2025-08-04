using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSwitch : MonoBehaviour
{
    public static WeaponSwitch Instance { get; private set; }

    [Header("Player's Weapon Inventory")]
    public int currWeaponIndex; // holds the index of the current weapon [0-primary, 1-secondary]
    public GameObject currWeapon;

    public GameObject[] weaponInventory; // array of guns in player's inventory
    public GameObject playerHand; 

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (weaponInventory == null || weaponInventory.Length != 2)
        { 
            weaponInventory = new GameObject[2]; 
        }
    }

    public void AddWeaponInInventory(GameObject weapon)
    {

        WeaponType type = weapon.GetComponent<Weapon>().weaponType;

        switch (type)
        {
            case WeaponType.Pistol:
                currWeaponIndex = 1; // secondary slot only
                break;
            case WeaponType.AutomaticRifle: //primary slot either guns
            case WeaponType.Shotgun:
                currWeaponIndex = 0;
                break;
        }

        if (weaponInventory[currWeaponIndex] != null)
        {
            ReplaceWeapon(weapon);
        }

        // disable the other slot's weapon when not equipped
        int otherSlot = 1 - currWeaponIndex;
        if (weaponInventory[otherSlot] != null)
        {
            weaponInventory[otherSlot].SetActive(false);
        }

        weaponInventory[currWeaponIndex] = weapon;
        weapon.SetActive(true);
        currWeapon = weapon;

    }

    private void ReplaceWeapon(GameObject weapon)
    {
        int replaceIndex = currWeaponIndex; // replace the index of the current weapon

        if (weaponInventory[replaceIndex] != null)
        {
            GameObject weaponToReplace = weaponInventory[replaceIndex];
            weaponInventory[replaceIndex] = null;

            if (weaponToReplace != null)
            {
                Destroy(weaponToReplace.gameObject);
            }
        }

        weaponInventory[replaceIndex] = weapon;
        currWeapon = weapon;
        weapon.SetActive(true);
    }

    public void SwitchToPrimaryWeapon()
    {
        if (weaponInventory[0] != null) 
        {
            if (weaponInventory[1] != null)
            {
                weaponInventory[1].SetActive(false);
            }

            currWeaponIndex = 0;
            currWeapon = weaponInventory[0];
            currWeapon.SetActive(true);

            Player.Instance.UpdateEquippedWeapon(currWeapon);
        }
    }

    public void SwitchToSecondaryWeapon()
    {
        if (weaponInventory[1] != null)
        {
            if (weaponInventory[0] != null)
            {
                weaponInventory[0].SetActive(false);
            }

            currWeaponIndex = 1;
            currWeapon = weaponInventory[1];
            currWeapon.SetActive(true);

            Player.Instance.UpdateEquippedWeapon(currWeapon);
        }
    }
}
