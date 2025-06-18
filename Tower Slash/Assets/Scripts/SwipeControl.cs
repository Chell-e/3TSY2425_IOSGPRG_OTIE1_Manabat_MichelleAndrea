using UnityEngine;

public class SwipeControl : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;

    [SerializeField] private float swipeThreshold = 20f;

    public Player player;
    public ArrowDirection swipeDirection;
    public bool hasSwiped = false;

    public static SwipeControl Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
#if UNITY_EDITOR_WIN
        MouseInput();
#elif UNITY_ANDROID
        TouchInput();
#endif
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            SwipeHandler();
            hasSwiped = true;
        }
    }

    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = Input.mousePosition;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                endPos = Input.mousePosition;
                SwipeHandler();
                hasSwiped = true;
            }
        }
    }

    void SwipeHandler()
    {
        Vector2 swipeDir = endPos - startPos;

        if (swipeDir.magnitude < swipeThreshold)
            return;

        float x = Mathf.Abs(swipeDir.x);
        float y = Mathf.Abs(swipeDir.y);

        if (x > y)
        {
            if (swipeDir.x > 0)
            {
                swipeDirection = ArrowDirection.Right;
                Debug.Log("Swipe RIGHT");
                player.Movement(Direction.Right);
            }
            else
            {
                swipeDirection = ArrowDirection.Left;
                Debug.Log("Swipe LEFT");
                player.Movement(Direction.Left);
            }
        }
        else
        {
            if (swipeDir.y > 0)
            {
                swipeDirection = ArrowDirection.Up;
                Debug.Log("Swipe UP");
                player.Movement(Direction.Up);
            }
            else
            {
                swipeDirection = ArrowDirection.Down;
                Debug.Log("Swipe DOWN");
                player.Movement(Direction.Down);
            }
        }
    }

    public ArrowDirection GetSwipeDirection()
    {
        return swipeDirection;
    }
}
