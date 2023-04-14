using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject circlePrefab;
    public EnemyController enemyPrefab;
    public GameObject dugTile;
    public PlayerController playerController;

    private bool showDebugMarkers = false;

    private int rowCount = 20;
    private int colCount = 11;
    private float spacing = .5f;

    private Vector3[,] gridPositions;
    private List<Vector3> dugTilePositions = new List<Vector3>();
    private List<EnemyController> enemeis = new List<EnemyController>();
    private Transform dugTilesContainer;

    // the bounds of the grid in the following order (minx, miny, maxx, maxy)
    private Vector4 bounds = Vector4.zero;

    void Start()
    {
        dugTilesContainer = new GameObject("Dug Tiles Container").transform;

        playerController.gridSpacing = spacing;

        InitGrid();
        PopulateEnemies();
    }

    void Update()
    {
        List<EnemyController> deadEnemies = new List<EnemyController>();

        foreach (var enemy in enemeis)
        {
            if (enemy.IsDead)
            {
                deadEnemies.Add(enemy);
            }
            else
            {
                enemy.MoveTowardsPosition(playerController.transform.position, dugTilePositions);
            }
        }

        if (deadEnemies.Count > 0)
        {
            foreach (var enemy in deadEnemies)
            {
                enemeis.Remove(enemy);
                Destroy(enemy.gameObject);
            }
        }
    }

    public Vector3[,] CreateGrid(int rows, int cols, float spacing, GameObject circlePrefab, bool showDebugMarkers)
    {
        Vector3[,] positions = new Vector3[rows, cols];

        Vector3 center = new Vector3((cols - 1) * spacing / 2f, (rows - 1) * spacing / 2f, 0f);

        GameObject gridContainer = null;
        if (showDebugMarkers)
        {
            gridContainer = new GameObject("Grid Container");
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 position = new Vector3(j * spacing, i * spacing, 0) - center;
                positions[i, j] = position;

                if (showDebugMarkers)
                {
                    GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
                    circle.transform.parent = gridContainer.transform;
                }
            }
        }

        return positions;
    }

    public void PreDigLine(GameObject obj, Vector3 pointA, Vector3 pointB, float threshold)
    {
        // Make sure pointA and pointB share either their X or Y values
        if (pointA.x != pointB.x && pointA.y != pointB.y)
        {
            Debug.LogError("Point A and B do not share either their X or Y values");
            return;
        }

        // Determine the direction of the line between pointA and pointB
        Vector3 direction = pointB - pointA;
        direction.Normalize();

        // Calculate the distance between the two points
        float distance = Vector3.Distance(pointA, pointB);

        // Instantiate objects every threshold distance between the two points
        for (float i = threshold; i < distance; i += threshold)
        {
            // Calculate the position of the current object to instantiate
            Vector3 position = pointA + (direction * i);

            Dig(position);
        }
    }

    public void Dig(Vector3 position)
    {
        GameObject newTile = Instantiate(dugTile, position, Quaternion.identity);

        if (dugTilesContainer != null)
        {
            newTile.transform.parent = dugTilesContainer;
        }

        dugTilePositions.Add(position);
    }

    public void InitGrid()
    {
        gridPositions = CreateGrid(
            rowCount,
            colCount,
            spacing,
            circlePrefab,
            showDebugMarkers);

        bounds = GetGridBounds(gridPositions);

        playerController.gridPositions = gridPositions;
        playerController.transform.position = gridPositions[rowCount - 3, 0];
    }

    public void PopulateEnemies()
    {
        var preDigPointA = gridPositions[3, 2];
        var preDigPointB = gridPositions[3, 6];
        PreDigLine(dugTile, preDigPointA, preDigPointB, .33f);

        var enemy = Instantiate(enemyPrefab, preDigPointA, Quaternion.identity);
        enemeis.Add(enemy);
    }

    public Vector4 GetGridBounds(Vector3[,] array)
    {
        int numRows = array.GetLength(0);
        int numCols = array.GetLength(1);

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                Vector3 point = array[row, col];
                if (point.x < minX)
                {
                    minX = point.x;
                }
                if (point.y < minY)
                {
                    minY = point.y;
                }
                if (point.x > maxX)
                {
                    maxX = point.x;
                }
                if (point.y > maxY)
                {
                    maxY = point.y;
                }
            }
        }

        return new Vector4(minX, minY, maxX, maxY);
    }

    public Vector3 ClampPositionToBounds(Vector3 point)
    {
        float x = Mathf.Clamp(point.x, bounds.x, bounds.z);
        float y = Mathf.Clamp(point.y, bounds.y, bounds.w);
        return new Vector3(x, y, point.z);
    }
}
