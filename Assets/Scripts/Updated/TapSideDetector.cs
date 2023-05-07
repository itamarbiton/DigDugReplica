using UnityEngine;
using System;

namespace Updated
{
    public class TapSideDetector : MonoBehaviour
    {
        public static event Action<ScreenSide> SideTapped;

        public enum ScreenSide
        {
            Left,
            Right
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = touch.position;
                    ScreenSide tappedSide = GetTappedScreenSide(touchPosition);
                    SideTapped?.Invoke(tappedSide);
                }
            }
        }

        private ScreenSide GetTappedScreenSide(Vector2 touchPosition)
        {
            float halfScreenWidth = Screen.width / 2f;

            if (touchPosition.x < halfScreenWidth)
            {
                return ScreenSide.Left;
            }
            else
            {
                return ScreenSide.Right;
            }
        }
    }

}