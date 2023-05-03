using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreenController : MonoBehaviour
{
    public static event Action PlayerDidRestart;

    private Animator animator;

    private bool didPlayerDie;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        Grid3DPlayerController.PlayerDidDie += OnPlayerDead;
    }

    private void Update()
    {
        if (didPlayerDie && Input.anyKeyDown)
        {
            PlayerDidRestart?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Grid3DPlayerController.PlayerDidDie -= OnPlayerDead;
    }

    private void OnPlayerDead()
    {
        StartCoroutine(EnterCoroutine());
        didPlayerDie = true;
    }

    private IEnumerator EnterCoroutine()
    {
        yield return new WaitForSeconds(.7f);
        
        animator.SetTrigger("Start");
    }
}
