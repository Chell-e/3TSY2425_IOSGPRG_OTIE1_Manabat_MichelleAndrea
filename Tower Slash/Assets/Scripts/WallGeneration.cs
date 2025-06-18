using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    public GameObject wallTile;
    public GameObject nextSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WallTrigger"))
        {
            Instantiate(wallTile, nextSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
