using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] public GameObject gridPrefab;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing;
    
    public Vector2[,] gridData { get; private set; }

    public void InitializeGrid()
    {
        gridData = GenerateGridData();
        InstantiateGrid(gridData);
    }

    private Vector2[,] GenerateGridData()
    {
        Vector2[,] gridData = new Vector2[rows, columns];

        var position = transform.position;
        Vector2 gridStartPosition = new Vector2(
            position.x - (columns * spacing) / 2 + spacing / 2,
            position.y - (rows * spacing) / 2 + spacing / 2);
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2 cellPosition = new Vector2(
                    gridStartPosition.x + j * spacing,
                    gridStartPosition.y + i * spacing);

                gridData[i, j] = cellPosition;
            }
        }

        return gridData;
    }

    private void InstantiateGrid(Vector2[,] gridData)
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