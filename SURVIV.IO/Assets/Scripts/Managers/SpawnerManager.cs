using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance {  get; private set; }

    [SerializeField] GameObject[] ammoPrefab;

    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float minPos;
    [SerializeField] float maxPos;

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
        StartCoroutine(AmmoSpawn());
    }
}
