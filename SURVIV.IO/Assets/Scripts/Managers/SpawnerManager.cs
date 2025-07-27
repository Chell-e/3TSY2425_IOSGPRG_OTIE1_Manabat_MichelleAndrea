using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [Header("Spawnable Prefabs")]
    [SerializeField] GameObject[] ammoPrefabs;
    [SerializeField] GameObject[] weaponPrefabs;

    [Header("Items to spawn")]
    [SerializeField] private int maxTotalItemsOnMap;

    [Header("Chances of spawning")]
    private float ammoSpawnChance = 70f;
    private float weaponSpawnChance = 30f;

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
        StartCoroutine(OverallSpawnRoutine());
    }

    IEnumerator OverallSpawnRoutine()
    {
        while (true)
        {
            currAmmoSpawned.RemoveAll(item => item == null);
            currWeaponSpawned.RemoveAll(item => item == null);

            int currentTotalItems = currAmmoSpawned.Count + currWeaponSpawned.Count;
            int itemsNeeded = maxTotalItemsOnMap - currentTotalItems;

            if (itemsNeeded > 0)
            {
                for (int i = 0; i < itemsNeeded; i++)
                {
                    float rand = Random.Range(0f, 100f); 

                    if (rand < weaponSpawnChance)
                    {
                        SpawnWeapon();
                    }
                    else
                    {
                        SpawnAmmo();
                    }
                }
            }

            yield return new WaitForSeconds(2f);
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
