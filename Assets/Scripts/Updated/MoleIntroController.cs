using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleIntroController : MonoBehaviour
{
    private Animator animator;

    private float nextPeekTime;
    private float initialWaitTime;

    private bool isAnimating = false;

    private float minWaitTime = 2f;
    private float maxWaitTime = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        initialWaitTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < initialWaitTime) return;

        if (Time.time >= nextPeekTime && !isAnimating)
        {
            StartCoroutine(Peek());
        }
    }

    private IEnumerator Peek()
    {
        isAnimating = true;
        
        animator.SetTrigger("Peek");

        yield return new WaitForSeconds(2.3f);

        nextPeekTime = Time.time + Random.Range(minWaitTime, maxWaitTime);

        isAnimating = false;
    }
}
