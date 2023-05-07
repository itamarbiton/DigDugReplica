using System;
using System.Collections.Generic;
using UnityEngine;
using Updated;
using Updated.Utilities;
using Random = UnityEngine.Random;

public class GridGenerator3D : MonoBehaviour
{
    public static event Action<GridGenerator3D> GridDidLoad;
    public static event Action AllGrassBladesCut;

    [SerializeField] public int rows;
    [SerializeField] public int columns;

    public Vector3[,] GridData { get; private set; }
    public GameObject[,] GridGameObjects { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> gridPrefabs;

    [SerializeField] private float spacing;
    
    private Grid3DPlayerController player;
    private EnemyManager enemyManager;

    private bool isGameRunning;
    private int grassBladesCount;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        InitializeGrid();

        Vector3 startPos = GridData[0, 0];
        Vector3 startPos3 = new Vector3(startPos.x, 0, startPos.z);
        Quaternion startRot = Quaternion.Euler(Vector3.up * 90);
        GameObject playerGameObject = Instantiate(playerPrefab, startPos3, startRot);
        player = playerGameObject.GetComponent<Grid3DPlayerController>();
        player.GridData = GridData;

        enemyManager = GetComponent<EnemyManager>();
    }

    private void Update()
    {
        if (!isGameRunning) return;
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (!isGameRunning) return;
        HandlePhysics();        
    }

    public void InitializeGrid()
    {
        GridData = GenerateGridData();
        
        InstantiateGrid(GridData);

        GridDidLoad?.Invoke(this);
    }

    private Vector3[,] GenerateGridData()
    {
        Vector3[,] gridData = new Vector3[rows, columns];

        var position = transform.position;
        Vector3 gridStartPosition = new Vector3(
            position.x - (columns * spacing) / 2 + spacing / 2,
            0,
            position.z - (rows * spacing) / 2 + spacing / 2);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 cellPosition = new Vector3(
                    gridStartPosition.x + j * spacing,
                    0,
                    gridStartPosition.z + i * spacing);

                gridData[i, j] = cellPosition;
            }
        }

        return gridData;
    }

    private void InstantiateGrid(Vector3[,] gridData)
    {
        int rows = gridData.GetLength(0);
        int cols = gridData.GetLength(1);

        GridGameObjects = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Instantiate the cell prefab at the stored position
                GameObject selectedPrefab = gridPrefabs[Random.Range(0, gridPrefabs.Count)]; 
                GameObject gridCell = Instantiate(selectedPrefab, gridData[i, j], Quaternion.identity, transform);
                
                GridGameObjects.SetValue(gridCell, i, j);
            }
        }

        grassBladesCount = GameObject.FindGameObjectsWithTag("GrassBlade").Length;
    }

    #region Game Handling

    private void HandleMovement()
    {
        player.HandleMovement();

        if (enemyManager != null)
        {
            enemyManager.HandleMovement();
        }
    }

    private void HandlePhysics()
    {
        player.HandleRotation();
        player.HandleDrive();
    }

    #endregion

    #region Event Handling

    private void SubscribeEvents()
    {
        InstructionsController.InstructionsFinished += OnFinishedInstructions;
        GrassBladeController.GrassBladeDidCut += OnGrassBladeCut;
        TapSideDetector.SideTapped += OnSideTapped;
    }

    private void UnsubscribeEvents()
    {
        InstructionsController.InstructionsFinished -= OnFinishedInstructions;
        GrassBladeController.GrassBladeDidCut -= OnGrassBladeCut;
        TapSideDetector.SideTapped -= OnSideTapped;
    }

    private void OnFinishedInstructions()
    {
        isGameRunning = true;
    }

    private void OnGrassBladeCut()
    {
        grassBladesCount -= 1;
        
        if (grassBladesCount <= 0)
        {
            AllGrassBladesCut?.Invoke();
        }
    }
    
    private void OnSideTapped(TapSideDetector.ScreenSide side)
    {
        if (!isGameRunning) return;
        
        player.ChangeDirection(side);
    }

    #endregion
}