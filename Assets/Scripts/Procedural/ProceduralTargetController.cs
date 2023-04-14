using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLimbController : MonoBehaviour
{
    [SerializeField]
    private Transform reference;

    [SerializeField]
    private float distanceTreshold = .5f;

    [SerializeField]
    private float animationDuration = .2f;

    private bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimating)
        {
            if (IsDistanceGreaterThanThreshold(gameObject, reference.position, distanceTreshold))
            {
                StartCoroutine(AnimateToPosition(gameObject, reference.position, animationDuration));
            }
        }
    }

    IEnumerator AnimateToPosition(GameObject gameObject, Vector3 targetPosition, float duration)
    {
        isAnimating = true;

        Vector3 startPosition = gameObject.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;

        isAnimating = false;
    }

    bool IsDistanceGreaterThanThreshold(GameObject gameObject, Vector3 point, float threshold)
    {
        float distance = Vector3.Distance(gameObject.transform.position, point);
        return distance > threshold;
    }
}
