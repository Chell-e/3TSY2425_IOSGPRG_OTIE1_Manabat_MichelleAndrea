using UnityEngine;

public enum ArrowDirection
{
    Right,
    Left,
    Up,
    Down,
}

public enum ArrowType
{
    Green,
    Red,
}

public class Arrow : MonoBehaviour
{
    public ArrowDirection arrowDirection;
    public ArrowType arrowType;

    public GameObject enemy;
    public bool isCorrectSwipe = false;

    void Start()
    {
        arrowDirection = (ArrowDirection)Random.Range(0, 4);
        SetArrowRotation();
    }
    
    void SetArrowRotation()
    {
        switch(arrowDirection)
        {
            case ArrowDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case ArrowDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case ArrowDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case ArrowDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SwipeControl.Instance.hasSwiped = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (SwipeControl.Instance.hasSwiped)
            {
                ArrowDirection swipeDirection = SwipeControl.Instance.GetSwipeDirection();

                Debug.Log("Arrow Direction: " + arrowDirection);
                Debug.Log("Swipe Direction: " + swipeDirection);

                if (arrowType == ArrowType.Green)
                {
                    isCorrectSwipe = (swipeDirection == arrowDirection);
                }
                else if (arrowType == ArrowType.Red)
                {
                    isCorrectSwipe = (swipeDirection == GetOppositeDirection(arrowDirection));
                }

                if (isCorrectSwipe)
                {
                    Debug.Log("Correct swipe!");
                    Destroy(gameObject);

                    //if (enemy != null)
                    //{
                    //    Destroy(enemy);
                    //}
                }
                else
                {
                    Debug.Log("Incorrect swipe!");
                }
            }
        }
    }

    ArrowDirection GetOppositeDirection(ArrowDirection dir)
    {
        switch(dir)
        {
            case ArrowDirection.Up: 
                return ArrowDirection.Down;
            case ArrowDirection.Down: 
                return ArrowDirection.Up; 
            case ArrowDirection.Right:
                return ArrowDirection.Left;
            case ArrowDirection.Left:
                return ArrowDirection.Right;
            default:
                return dir;
        }
    }
}
