using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineGridTargetController : MonoBehaviour
{
    [SerializeField] private float targetRadius = 0.2f;
    
    private CinemachineTargetGroup targetGroup;

    void OnEnable()
    {
        SubscribeEvents();
        
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #region Event Handling

    private void SubscribeEvents()
    {
        GridGenerator3D.GridDidLoad += OnGridLoad;
    }

    private void UnsubscribeEvents()
    {
        GridGenerator3D.GridDidLoad -= OnGridLoad;
    }

    private void OnGridLoad(GridGenerator3D gridGenerator)
    {
        targetGroup.m_Targets = new CinemachineTargetGroup.Target[2];
        
        targetGroup.m_Targets.SetValue(new CinemachineTargetGroup.Target
        {
            target = gridGenerator.GridGameObjects[0, 0].transform,
            weight = 1,
            radius = targetRadius
        }, 0);

        var rows = gridGenerator.GridGameObjects.GetLength(0);
        var cols = gridGenerator.GridGameObjects.GetLength(1);
        
        targetGroup.m_Targets.SetValue(new CinemachineTargetGroup.Target
        {
            target = gridGenerator.GridGameObjects[rows - 1, cols - 1].transform,
            weight = 1,
            radius = targetRadius
        }, 1);
    }

    #endregion
}
