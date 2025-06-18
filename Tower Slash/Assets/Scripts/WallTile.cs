using UnityEngine;

public class WallTile : MonoBehaviour
{
    private WallSpawner wallSpawner;

    private void Start()
    {
        wallSpawner = Object.FindFirstObjectByType<WallSpawner>();
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            wallSpawner.SpawnTile();
            Destroy(gameObject, 1);
        }
    }

}
