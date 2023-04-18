using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameBoss : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    
    private GridGenerator gridGenerator;
    private GridPlayerController player;

    private void Start()
    {
        gridGenerator = GetComponent<GridGenerator>();
        gridGenerator.InitializeGrid();
        
        InstantiatePlayer();
    }

    private void Update()
    {
        player.HandleInput();
        player.HandleMovement();
    }

    private void InstantiatePlayer()
    {
        Vector2 playerPos = gridGenerator.gridData[0, 0];
        GameObject playerGameObject = Instantiate(playerPrefab, new Vector3(playerPos.x, playerPos.y, 0), Quaternion.identity);
        player = playerGameObject.GetComponent<GridPlayerController>();
        player.GridData = gridGenerator.gridData;
    }
}
