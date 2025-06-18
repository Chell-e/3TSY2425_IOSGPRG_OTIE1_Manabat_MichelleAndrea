using UnityEngine;

public enum GameState
{
    GameInit,
    GameStart,
    GamePause,
    GameEnd,
}

public class GameManager : Singleton<GameManager>
{
    public GameState GameState { get; private set; }

    public GameObject player;
    public GameObject mainCamera;
    public GameObject wall;
    public GameObject spawnFollower;

    private void Start()
    {
        GameState = GameState.GameInit;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Game Started!");
            GameStart();
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Game Ended!");
            GameEnded();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("Game Reset!");
            GameRestart();
        }
    }

    public void GameStart()
    {
        GameState = GameState.GameStart;
        SpawnerController.Instance.StartSpawning();
    }

    public void GameRestart()
    {
        GameState = GameState.GameStart;
        SpawnerController.Instance.StopSpawning();
        SpawnerController.Instance.Reset();
        SpawnerController.Instance.StartSpawning();

        player.GetComponent<Player>().Reset();
        mainCamera.GetComponent<CameraFollow>().Reset();
        wall.GetComponent<WallSpawner>().Reset();
        spawnFollower.GetComponent<SpawnerController>().Reset();
        // restart spawner & everything else here
    }

    public void GameEnded()
    {
        GameState = GameState.GameEnd;
        SpawnerController.Instance.StopSpawning();
    }
}
