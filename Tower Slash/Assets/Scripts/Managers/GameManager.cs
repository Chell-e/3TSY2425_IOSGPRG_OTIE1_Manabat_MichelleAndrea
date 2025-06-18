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
    }

    public void GameRestart()
    {
        player.GetComponent<Player>().Reset();
        mainCamera.GetComponent<CameraFollow>().Reset();
        wall.GetComponent<WallSpawner>().Reset();
        // restart spawner & everything else here
    }

    public void GameEnded()
    {
        GameState = GameState.GameEnd;
    }
}
