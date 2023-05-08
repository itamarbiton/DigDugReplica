using UnityEngine;
using UnityEngine.EventSystems;

public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform joystickArea;
    [SerializeField] private RectTransform joystick;
    [Range(0, 1)] [SerializeField] private float deadZone = 0.2f;

    private Vector2 initialTouchPosition;
    public static bool IsTouching;

    public enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public static Direction CurrentDirection { get; private set; } = Direction.None;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsTouching = true;
        initialTouchPosition = eventData.position;
        joystickArea.position = initialTouchPosition;
        joystickArea.gameObject.SetActive(true);
        UpdateJoystick(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsTouching = false;
        joystickArea.gameObject.SetActive(false);
        joystick.anchoredPosition = Vector2.zero;
        CurrentDirection = Direction.None;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsTouching)
        {
            UpdateJoystick(eventData.position);
        }
    }

    private void UpdateJoystick(Vector2 newPosition)
    {
        Vector2 delta = newPosition - initialTouchPosition;
        joystick.anchoredPosition = delta;

        float angle = Vector2.SignedAngle(Vector2.right, delta);

        if (delta.magnitude > joystickArea.sizeDelta.x * deadZone)
        {
            if (angle >= 45 && angle <= 135)
            {
                CurrentDirection = Direction.Up;
            }
            else if (angle <= -45 && angle >= -135)
            {
                CurrentDirection = Direction.Down;
            }
            else if (angle >= -45 && angle <= 45)
            {
                CurrentDirection = Direction.Right;
            }
            else if (angle <= 180 && angle >= 135 || angle >= -180 && angle <= -135)
            {
                CurrentDirection = Direction.Left;
            }
        }
        else
        {
            CurrentDirection = Direction.None;
        }
    }
}
