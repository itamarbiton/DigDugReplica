using System;
using UnityEngine;
using Updated;

public class WinConditionController : MonoBehaviour
{
    public static event Action PlayerDidWin;
    
    private int grassBladesCount;
    
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }
    
    private void SubscribeEvents()
    {
        GridManager.GridDidLoad += OnGridDidLoad;
        GrassBladeController.GrassBladeDidCut += OnGrassBladeCut;
    }

    private void UnsubscribeEvents()
    {
        GridManager.GridDidLoad += OnGridDidLoad;
        GrassBladeController.GrassBladeDidCut -= OnGrassBladeCut;
    }

    private void OnGridDidLoad(GridManager gridManager)
    {
        grassBladesCount = GameObject.FindGameObjectsWithTag("GrassBlade").Length;
    }

    private void OnGrassBladeCut()
    {
        grassBladesCount -= 1;
        
        if (grassBladesCount <= 0)
        {
            PlayerDidWin?.Invoke();
        }
    }
}