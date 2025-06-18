using UnityEngine;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    private Vector3 originalPos;

    public GameObject wallTile;
    Vector3 nextSpawnPoint;

    private List<GameObject> spawnedWalls = new List<GameObject>();

    public void SpawnTile()
    {
        GameObject temp = Instantiate(wallTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;

        spawnedWalls.Add(temp);
    }

    private void Start()
    {
        originalPos = transform.position;
        GameManager.Instance.wall = this.gameObject;

        InitializeTiles();
    }

    private void InitializeTiles()
    {
        transform.position = originalPos;
        nextSpawnPoint = transform.position;
        for (int i = 0; i < 10; i++)
        {
            SpawnTile();
        }
    }

    public void Reset()
    {
        foreach (GameObject wall in spawnedWalls)
        {
            if (wall != null)
            {
                Destroy(wall);
            }
        }
        spawnedWalls.Clear();

        InitializeTiles();
    }
}
