using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridGenerator3D : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private float enemy1Percent = .2f;
    [SerializeField] private List<GameObject> gridPrefabs;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing;

    public Vector3[,] gridData { get; private set; }

    private Grid3DPlayerController player;
    private List<GameObject> enemies;

    private bool isGameRunning = false;

    private void Start()
    {
        InitializeGrid();

        Vector3 startPos = gridData[0, 0];
        Vector3 startPos3 = new Vector3(startPos.x, 0, startPos.z);
        Quaternion startRot = Quaternion.Euler(Vector3.up * 90);
        GameObject playerGameObject = Instantiate(playerPrefab, startPos3, startRot);
        player = playerGameObject.GetComponent<Grid3DPlayerController>();
        player.GridData = gridData;

        InstructionsController.InstructionsFinished += OnFinishedInstructions;
    }

    private void OnDestroy()
    {
        InstructionsController.InstructionsFinished -= OnFinishedInstructions;
    }

    private void Update()
    {
        if (!isGameRunning) return;
        
        player.HandleInput();
        player.HandleMovement();

        foreach (var enemy in enemies)
            enemy.GetComponent<MoleController>().HandleMovement();
    }

    public void InitializeGrid()
    {
        gridData = GenerateGridData();
        InstantiateGrid(gridData);
        InstantiateEnemies(gridData);
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
        for (int i = 0; i < gridData.GetLength(0); i++)
        {
            for (int j = 0; j < gridData.GetLength(1); j++)
            {
                // Instantiate the cell prefab at the stored position
                GameObject selectedPrefab = gridPrefabs[Random.Range(0, gridPrefabs.Count)]; 
                GameObject gridCell = Instantiate(selectedPrefab, gridData[i, j], Quaternion.identity, transform);
            }
        }
    }

    private void InstantiateEnemies(Vector3[,] gridData)
    {
        List<int[]> enemyPositions = GetRandomIndices(gridData, enemy1Percent);

        List<GameObject> newEnemeies = new List<GameObject>();

        foreach (var enemyPos in enemyPositions)
        {
            Vector3 enemyWorldPos = gridData[enemyPos[0], enemyPos[1]];
            GameObject enemy1 = Instantiate(enemy1Prefab, enemyWorldPos, Quaternion.identity, transform);

            if (gridData != null)
            {
                var controller = enemy1.GetComponent<MoleController>(); 
                controller.GridData = gridData;
                controller.GridPosition = new Vector2(enemyPos[0], enemyPos[1]);
            }
            else
            {
                Debug.Log("[WARNING]: grid data is null while initializing enemy!");
            }
            
            newEnemeies.Add(enemy1);
        }

        enemies = newEnemeies;
    }

    private void OnFinishedInstructions()
    {
        isGameRunning = true;
    }
    
    public List<Vector3> GetRandomElements(Vector3[,] array, float percentage) {
        List<Vector3> result = new List<Vector3>();
        int totalElements = array.GetLength(0) * array.GetLength(1);
        int numElementsToPick = Mathf.RoundToInt(totalElements * percentage); // percentage of total elements
        int numElementsPicked = 0;
        System.Random random = new System.Random();

        while (numElementsPicked < numElementsToPick) {
            int randomX = random.Next(array.GetLength(0));
            int randomY = random.Next(array.GetLength(1));

            Vector3 randomElement = array[randomX, randomY];

            if (!result.Contains(randomElement)) {
                result.Add(randomElement);
                numElementsPicked++;
            }
        }

        return result;
    }
    
    public List<int[]> GetRandomIndices(Vector3[,] array, float percentage) {
        List<int[]> result = new List<int[]>();
        int totalElements = array.GetLength(0) * array.GetLength(1);
        int numElementsToPick = Mathf.RoundToInt(totalElements * percentage); // percentage of total elements
        int numElementsPicked = 0;
        System.Random random = new System.Random();

        while (numElementsPicked < numElementsToPick) {
            int randomX = random.Next(array.GetLength(0));
            int randomY = random.Next(array.GetLength(1));

            int[] randomIndex = new int[] { randomX, randomY };

            if (!result.Any(index => index.SequenceEqual(randomIndex))) {
                result.Add(randomIndex);
                numElementsPicked++;
            }
        }

        return result;
    }
}