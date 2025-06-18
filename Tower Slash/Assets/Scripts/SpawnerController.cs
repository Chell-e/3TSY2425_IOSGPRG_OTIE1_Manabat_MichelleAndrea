using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<GameObject> arrowPrefabs;

    [SerializeField] Transform enemySpawnPoint;
    [SerializeField] Transform arrowSpawnPoint;

    [SerializeField] float spawnInterval = 5; // 1 second
    Coroutine spawnRoutine;

    [SerializeField] List<GameObject> enemyList = new List<GameObject>();

    public static SpawnerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartSpawning()
    {
        spawnRoutine = StartCoroutine(Spawning());
    }

    public void Reset()
    {
        foreach (GameObject obj in enemyList)
        {
            Destroy(obj);
        }
        enemyList.Clear();
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnRoutine);
    }

    IEnumerator Spawning()
    {
        while(GameManager.Instance.GameState == GameState.GameStart)
        {
            SpawnObjects();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObjects()
    {
        SpawnEnemy();
        SpawnArrow();
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
        enemyList.Add(enemyObj);
    }

    void SpawnArrow()
    {
        int randomIndex = Random.Range(0, arrowPrefabs.Count);
        GameObject arrowObj = Instantiate(arrowPrefabs[randomIndex], arrowSpawnPoint.position, Quaternion.identity);
    }
}
