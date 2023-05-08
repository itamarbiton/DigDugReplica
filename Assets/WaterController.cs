using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    private Animator animator;

    public float waitMin = 2f;
    public float waitMax = 4f;
    
    private float waitTimer;
    private float waitDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        waitDuration = Random.Range(waitMin, waitMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer < waitDuration)
        {
            waitTimer += Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("Start");
            waitTimer = 0;
            waitDuration = Random.Range(waitMin, waitMax);
        }
    }
}
