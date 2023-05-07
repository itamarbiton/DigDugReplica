using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Updated
{
    public class GridManager : MonoBehaviour
    {
        public static event Action<GridManager> GridDidLoad;

        public Vector3[,] GridData => GetWorldPositionsGrid();
        public GameObject[,] GridGameObjects { get; private set; }

        [System.Serializable]
        public class SpecialPrefab
        {
            public GameObject prefab;
            public int count;
            public int width;
            public int height;
        }

        [SerializeField] private List<GameObject> normalPrefabs;
        [SerializeField] private List<SpecialPrefab> specialPrefabs;
        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private float gridCellSize = 1f;
        [SerializeField] private int seed;

        private bool[,] grid;

        #region Initialization

        public void InitializeGrid()
        {
            Random.InitState(seed);

            if (LevelDataProvider.currentLevel != null)
            {
                LevelDataProvider.LevelData currentLevel = LevelDataProvider.currentLevel;
                
                gridWidth = currentLevel.width;
                gridHeight = currentLevel.height;
                
                grid = new bool[currentLevel.width, currentLevel.height];
                GridGameObjects = new GameObject[currentLevel.width, currentLevel.height];
                
                normalPrefabs = currentLevel.normalPrefabs;
                specialPrefabs = currentLevel.specialPrefabs;
            }
            else
            {
                grid = new bool[gridWidth, gridHeight];
                GridGameObjects = new GameObject[gridWidth, gridHeight];
            }
            
            PlaceSpecialPrefabs();
            
            FillRemainingGridWithNormalPrefab();
            
            GridDidLoad?.Invoke(this);
        }
        
        #endregion

        #region Utilities

        public int GetWidth()
        {
            return gridWidth;
        }

        public int GetHeight()
        {
            return gridHeight;
        }

        public Vector3[,] GetWorldPositionsGrid()
        {
            Vector3[,] worldPositionsGrid = new Vector3[gridWidth, gridHeight];

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 position = new Vector3(
                        x * gridCellSize - gridWidth * gridCellSize / 2f + gridCellSize / 2f,
                        0,
                        y * gridCellSize - gridHeight * gridCellSize / 2f + gridCellSize / 2f);
                    worldPositionsGrid[x, y] = position;
                }
            }

            return worldPositionsGrid;
        }

        #endregion
        
        #region Private Implementation Details

        private void PlaceSpecialPrefabs()
        {
            foreach (SpecialPrefab specialPrefab in specialPrefabs)
            {
                for (int i = 0; i < specialPrefab.count; i++)
                {
                    PlaceSpecialPrefab(specialPrefab);
                }
            }
        }

        private void PlaceSpecialPrefab(SpecialPrefab specialPrefab)
        {
            bool placed = false;

            while (!placed)
            {
                int x = Random.Range(0, gridWidth - specialPrefab.width + 1);
                int y = Random.Range(0, gridHeight - specialPrefab.height + 1);

                if (IsAreaFree(x, y, specialPrefab.width, specialPrefab.height))
                {
                    for (int i = x; i < x + specialPrefab.width; i++)
                    {
                        for (int j = y; j < y + specialPrefab.height; j++)
                        {
                            grid[i, j] = true;
                        }
                    }

                    Vector3 position = new Vector3(
                        (x + specialPrefab.width / 2f - 0.5f) * gridCellSize - gridWidth * gridCellSize / 2f +
                        gridCellSize / 2f, 0,
                        (y + specialPrefab.height / 2f - 0.5f) * gridCellSize - gridHeight * gridCellSize / 2f +
                        gridCellSize / 2f);
                    Instantiate(specialPrefab.prefab, position, Quaternion.identity, transform);
                    placed = true;
                }
            }
        }

        private bool IsAreaFree(int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    if (grid[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void FillRemainingGridWithNormalPrefab()
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    if (!grid[x, y])
                    {
                        Vector3 position =
                            new Vector3(x * gridCellSize - gridWidth * gridCellSize / 2f + gridCellSize / 2f, 0,
                                y * gridCellSize - gridHeight * gridCellSize / 2f + gridCellSize / 2f);
                        
                        GameObject selectedPrefab = normalPrefabs[Random.Range(0, normalPrefabs.Count)];
                        GameObject normal = Instantiate(selectedPrefab, position, Quaternion.identity, transform);
                        
                        GridGameObjects.SetValue(normal, x, y);
                    }
                }
            }
        }
        
        #endregion
    }
}