using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    public static event Action PlayerDidWinRestart;
    
    private Animator animator;
    private bool playerDidWin;

    void Start()
    {
        animator = GetComponent<Animator>();

        GridGenerator3D.AllGrassBladesCut += OnAllGrassBladesCut;
    }

    private void Update()
    {
        if (playerDidWin && Input.GetKeyDown(KeyCode.Space))
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
        playerDidWin = true;
    }
}
