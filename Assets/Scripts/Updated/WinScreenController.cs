using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Updated;

public class WinScreenController : MonoBehaviour
{
    public static event Action PlayerDidWinRestart;

    [SerializeField] private ParticleSystem confettiParticleSystem;
    
    private Animator animator;
    private bool playerDidWin;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsusbscribeEvents();
    }

    private void SubscribeEvents()
    {
        WinConditionController.PlayerDidWin += PlayerDidWin;
    }

    private void UnsusbscribeEvents()
    {
        WinConditionController.PlayerDidWin -= PlayerDidWin;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerDidWin && Input.anyKeyDown)
        {
            PlayerDidWinRestart?.Invoke();
        }
    }

    private void PlayerDidWin()
    {
        animator.SetTrigger("Start");
        confettiParticleSystem.Play();
        CinemachineShake.Instance.ShakeCamera(5f, 1f);
        playerDidWin = true;
    }
}
