using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GamepadFollower : MonoBehaviour
{
    [SerializeField] private RectTransform gamepadImage;

    private RectTransform internalImage;

    // Start is called before the first frame update
    void Start()
    {
        internalImage = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            var touch = Input.touches.First();
            internalImage.anchoredPosition = gamepadImage.anchoredPosition;
        }
        else
        {
            internalImage.anchoredPosition = Vector2.Lerp(internalImage.anchoredPosition, gamepadImage.anchoredPosition, Time.deltaTime * 0.2f);
        }
    }
}
