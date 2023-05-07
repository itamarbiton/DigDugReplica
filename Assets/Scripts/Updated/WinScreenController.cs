using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    public static event Action PlayerDidWinRestart;

    [SerializeField] private ParticleSystem confettiParticleSystem;
    
    private Animator animator;
    private bool playerDidWin;

    void Start()
    {
        animator = GetComponent<Animator>();

        GridGenerator3D.AllGrassBladesCut += OnAllGrassBladesCut;
    }

    private void Update()
    {
        if (playerDidWin && Input.anyKeyDown)
        {
            PlayerDidWinRestart?.Invoke();
        }
    }

    private void OnDestroy()
    {
        GridGenerator3D.AllGrassBladesCut -= OnAllGrassBladesCut;
    }

    private void OnAllGrassBladesCut()
    {
        animator.SetTrigger("Start");
        confettiParticleSystem.Play();
        CinemachineShake.Instance.ShakeCamera(5f, 1f);
        playerDidWin = true;
    }
}
