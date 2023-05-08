using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public enum SwipeDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public static SwipeDetector Instance { get; private set; }

    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private float minSwipeSpeed = 500f;

    private Vector2 startTouchPosition;
    private float startTime;
    private SwipeDirection lastSwipeDirection = SwipeDirection.None;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 currentTouchPosition = touch.position;
                float elapsedTime = Time.time - startTime;
                float swipeSpeed = (currentTouchPosition - startTouchPosition).magnitude / elapsedTime;

                if (swipeSpeed >= minSwipeSpeed)
                {
                    Vector2 swipeDirection = (currentTouchPosition - startTouchPosition).normalized;
                    DetermineSwipeDirection(swipeDirection);
                    startTouchPosition = currentTouchPosition;
                    startTime = Time.time;
                }
            }
        }
    }

    private void DetermineSwipeDirection(Vector2 swipeDirection)
    {
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            if (swipeDirection.x > 0)
                lastSwipeDirection = SwipeDirection.Right;
            else
                lastSwipeDirection = SwipeDirection.Left;
        }
        else
        {
            if (swipeDirection.y > 0)
                lastSwipeDirection = SwipeDirection.Up;
            else
                lastSwipeDirection = SwipeDirection.Down;
        }
    }

    public SwipeDirection GetLastSwipeDirection()
    {
        return lastSwipeDirection;
    }
}