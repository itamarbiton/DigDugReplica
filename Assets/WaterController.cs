using System.Collections;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private Animator animator;

    public float waitMin = 4f;
    public float waitMax = 6f;
    
    private float waitTimer;
    private float waitDuration;

    private bool isAnimating;
    
    void Start()
    {
        animator = GetComponent<Animator>();

        waitDuration = Random.Range(waitMin, waitMax);
    }

    void Update()
    {
        if (waitTimer < waitDuration) waitTimer += Time.deltaTime;
        else  if (!isAnimating) StartCoroutine(StartAnimationCoroutine());
    }

    private IEnumerator StartAnimationCoroutine()
    {
        isAnimating = true;
        
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(5.1f);

        waitTimer = 0;
        waitDuration = Random.Range(waitMin, waitMax);
        
        isAnimating = false;
    }
}
