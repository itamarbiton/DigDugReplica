using System;
using UnityEngine;

public class GridGenerator3D : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing;
    
    public Vector3[,] gridData { get; private set; }

    private Grid3DPlayerController player;

    private void Start()
    {
        gridData = GenerateGridData();
        InstantiateGrid(gridData);

        Vector3 startPos = gridData[0, 0];
        Vector3 startPos3 = new Vector3(startPos.x, 1, startPos.z);
        GameObject playerGameObject = Instantiate(playerPrefab, startPos3, Quaternion.identity);
        player = playerGameObject.GetComponent<Grid3DPlayerController>();
        player.GridData = gridData;
    }

    private void Update()
    {
        player.HandleInput();
        player.HandleMovement();
    }

    public void InitializeGrid()
    {
        gridData = GenerateGridData();
        InstantiateGrid(gridData);
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
                GameObject gridCell = Instantiate(gridPrefab, gridData[i, j], Quaternion.identity, transform);
            }
        }
    }
}