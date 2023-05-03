using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private static SwipeDetector instance;

    public static SwipeDetector Instance { get { return instance; } }

    private Vector2 fingerDown;
    private Vector2 fingerUp;

    public bool detectSwipeOnlyAfterRelease = false;

    public float minDistanceForSwipe = 20f;
    private SwipeData lastSwipeData;

    public event System.Action<SwipeData> OnSwipe = delegate { };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            // Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("[TEST]: touch phase ended");
                fingerDown = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        // Check if we detected a swipe
        if (SwipeDistanceCheck())
        {
            // Which direction did we swipe?
            if (IsVerticalSwipe())
            {
                var direction = fingerDown.y - fingerUp.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDown.x - fingerUp.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            fingerUp = fingerDown;
        }
        else
        {
            Debug.Log($"[TEST] swipe not far enough, distance was {VerticalDistance()}");
        }
    }

    bool SwipeDistanceCheck()
    {
        bool didSwipe = VerticalDistance() > minDistanceForSwipe || HorizontalDistance() > minDistanceForSwipe;

        if (!didSwipe)
        {
            Debug.Log($"[TEST] detected touch, but no swipe, distance was {VerticalDistance()}");
        }

        return didSwipe;
    }

    bool IsVerticalSwipe()
    {
        return VerticalDistance() > HorizontalDistance();
    }

    float VerticalDistance()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float HorizontalDistance()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerUp,
            EndPosition = fingerDown
        };
        
        Debug.Log($"[TEST] swipe detected with direction: {swipeData.Direction}");

        lastSwipeData = swipeData;
        
        OnSwipe(swipeData);
    }
    
    public SwipeData GetLastSwipeData()
    {
        return lastSwipeData;
    }
}

public enum SwipeDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

public struct SwipeData
{
    public SwipeDirection Direction;
    public Vector2 StartPosition;
    public Vector2 EndPosition;
}
