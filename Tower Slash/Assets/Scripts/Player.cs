using UnityEngine;

public enum Direction
{
    Right,
    Left, 
    Up, 
    Down,
}

public class Player : MonoBehaviour
{
    private static Player _instance;

    [SerializeField] private float movementSpeed;
    //[SerializeField] private int life = 3;

    private Vector3 originalPos;

    public static Player Instance 
    { 
        get 
        { 
            if (_instance == null) 
            { 
                Debug.LogError("Player instance is null.");
                return null;
            } 

            return _instance; 
        } 
    }
 
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        originalPos = transform.position;
        GameManager.Instance.player = this.gameObject;
    }

    void Update()
    {
        transform.position += new Vector3(0, 1, 0) * Time.deltaTime * movementSpeed;
    }

    public void Movement(Direction dir)
    {
        if (GameManager.Instance.GameState != GameState.GameStart)
        {
            return;
        }

        switch (dir)
        {
            case Direction.Left:
                this.transform.position += Vector3.left;
                break;
            case Direction.Right:
                this.transform.position += Vector3.right;
                break;
            case Direction.Up:
                this.transform.position += Vector3.up;
                break;
            case Direction.Down:
                this.transform.position += Vector3.down;
                break;
        }
    }

    public void Reset()
    {
        this.transform.position = originalPos;
        //life = 3;
    }
}
