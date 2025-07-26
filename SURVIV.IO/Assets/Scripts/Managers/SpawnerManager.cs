using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [Header("Spawnable Prefabs")]
    [SerializeField] GameObject[] ammoPrefabs;
    [SerializeField] GameObject[] weaponPrefabs;

    [Header("Initialization")]
    [SerializeField] private int initialAmmo;
    [SerializeField] private int initialWeapon;

    [Header("Map Bounds")]
    [SerializeField] private Vector2 mapMinBounds2D;
    [SerializeField] private Vector2 mapMaxBounds2D;

    public List<GameObject> currAmmoSpawned = new List<GameObject>();
    public List<GameObject> currWeaponSpawned = new List<GameObject>();

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
        StartCoroutine(SpawnAmmoRoutine());
        StartCoroutine(SpawnWeaponRoutine());

    }

    IEnumerator SpawnAmmoRoutine()
    {
        while (true)
        {
            currAmmoSpawned.RemoveAll(item => item == null);

            int minAmmo = initialAmmo - currAmmoSpawned.Count;

            for (int i = 0; i < minAmmo; i++)
            {
                SpawnAmmo();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnWeaponRoutine()
    {
        while(true)
        {
            currWeaponSpawned.RemoveAll(item => item == null);

            int minWeapon = initialWeapon - currWeaponSpawned.Count;

            for (int i = 0; i < minWeapon; i++)
            {
                SpawnWeapon();
            }

            yield return new WaitForSeconds(5f);
        }
    }
    
    private void SpawnAmmo()
    {
        GameObject ammoObj = ammoPrefabs[Random.Range(0, ammoPrefabs.Length)];
        
        GameObject newAmmo = Instantiate(ammoObj, SpawnRandomPosition(), Quaternion.identity);

        currAmmoSpawned.Add(newAmmo);
    }

    private void SpawnWeapon()
    {
        GameObject weaponObj = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];

        GameObject newWeapon = Instantiate(weaponObj, SpawnRandomPosition(), Quaternion.identity);

        currWeaponSpawned.Add(newWeapon);
    }

    private Vector3 SpawnRandomPosition()
    {
        float randX = Random.Range(mapMinBounds2D.x, mapMaxBounds2D.x);
        float randY = Random.Range(mapMinBounds2D.y, mapMaxBounds2D.y);

        Vector3 spawnPos = new Vector3(randX, randY, 0f);

        return spawnPos;
    }


}
