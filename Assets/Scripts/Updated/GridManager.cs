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

        [System.Serializable]
        public class GridConfiguration
        {
            public List<GameObject> normalPrefabs;
            public List<SpecialPrefab> specialPrefabs;
            public int gridWidth;
            public int gridHeight;
            public float gridCellSize = 1f;
            public int seed;
            public float targetGroupRadius;
        }

        [SerializeField] private GridConfiguration config;

        private bool[,] grid;

        #region Gizmos

        private void OnDrawGizmos()
        {
            Random.InitState(config.seed);

            bool[,] gizmoGrid = new bool[config.gridWidth, config.gridHeight];
            GameObject[,] gizmoGridGameObjects = new GameObject[config.gridWidth, config.gridHeight];

            PlaceSpecialPrefabs(gizmoGrid, new GizmoGridInstanceProvider(Color.red, config.gridCellSize, .5f));

            FillRemainingGridWithNormalPrefab(gizmoGrid, gizmoGridGameObjects,
                new GizmoGridInstanceProvider(Color.green, config.gridCellSize, .1f));
        }

        #endregion

        #region Initialization

        public void InitializeGrid()
        {
            Random.InitState(config.seed);

            grid = new bool[config.gridWidth, config.gridHeight];
            GridGameObjects = new GameObject[config.gridWidth, config.gridHeight];

            PlaceSpecialPrefabs(grid, new ObjectsGridInstanceProvider());

            FillRemainingGridWithNormalPrefab(grid, GridGameObjects, new ObjectsGridInstanceProvider());

            GridDidLoad?.Invoke(this);
        }

        #endregion

        #region Configuration

        public void SetConfiguration(GridConfiguration config)
        {
            this.config = config;
        }

        #endregion

        #region Utilities

        public int GetWidth()
        {
            return config.gridWidth;
        }

        public int GetHeight()
        {
            return config.gridHeight;
        }

        public GridConfiguration GetConfiguration()
        {
            return config;
        }

        public Vector3[,] GetWorldPositionsGrid()
        {
            Vector3[,] worldPositionsGrid = new Vector3[config.gridWidth, config.gridHeight];

            for (int x = 0; x < config.gridWidth; x++)
            {
                for (int y = 0; y < config.gridHeight; y++)
                {
                    Vector3 position = new Vector3(
                        x * config.gridCellSize - config.gridWidth * config.gridCellSize / 2f +
                        config.gridCellSize / 2f,
                        0,
                        y * config.gridCellSize - config.gridHeight * config.gridCellSize / 2f +
                        config.gridCellSize / 2f);
                    worldPositionsGrid[x, y] = position;
                }
            }

            return worldPositionsGrid;
        }

        #endregion

        #region Private Implementation Details

        private void PlaceSpecialPrefabs(bool[,] grid, IGridInstanceProvider instanceProvider)
        {
            foreach (SpecialPrefab specialPrefab in config.specialPrefabs)
            {
                for (int i = 0; i < specialPrefab.count; i++)
                {
                    PlaceSpecialPrefab(grid, specialPrefab, instanceProvider);
                }
            }
        }

        private void PlaceSpecialPrefab(bool[,] grid, SpecialPrefab specialPrefab,
            IGridInstanceProvider instanceProvider)
        {
            bool placed = false;

            while (!placed)
            {
                int x = Random.Range(0, config.gridWidth - specialPrefab.width + 1);
                int y = Random.Range(0, config.gridHeight - specialPrefab.height + 1);

                if (IsAreaFree(grid, x, y, specialPrefab.width, specialPrefab.height))
                {
                    for (int i = x; i < x + specialPrefab.width; i++)
                    {
                        for (int j = y; j < y + specialPrefab.height; j++)
                        {
                            grid[i, j] = true;
                        }
                    }

                    Vector3 position = new Vector3(
                        (x + specialPrefab.width / 2f - 0.5f) * config.gridCellSize -
                        config.gridWidth * config.gridCellSize / 2f +
                        config.gridCellSize / 2f, 0,
                        (y + specialPrefab.height / 2f - 0.5f) * config.gridCellSize -
                        config.gridHeight * config.gridCellSize / 2f +
                        config.gridCellSize / 2f);

                    instanceProvider.Instantiate(specialPrefab.prefab, position, Quaternion.identity, transform);

                    placed = true;
                }
            }
        }

        private bool IsAreaFree(bool[,] grid, int startX, int startY, int width, int height)
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

        private void FillRemainingGridWithNormalPrefab(bool[,] grid, GameObject[,] gridGameObjects,
            IGridInstanceProvider instanceProvider)
        {
            for (int x = 0; x < config.gridWidth; x++)
            {
                for (int y = 0; y < config.gridHeight; y++)
                {
                    if (!grid[x, y])
                    {
                        Vector3 position =
                            new Vector3(
                                x * config.gridCellSize - config.gridWidth * config.gridCellSize / 2f +
                                config.gridCellSize / 2f, 0,
                                y * config.gridCellSize - config.gridHeight * config.gridCellSize / 2f +
                                config.gridCellSize / 2f);

                        GameObject selectedPrefab = config.normalPrefabs[Random.Range(0, config.normalPrefabs.Count)];

                        GameObject normal = instanceProvider.Instantiate(selectedPrefab, position, Quaternion.identity,
                            transform);

                        gridGameObjects.SetValue(normal, x, y);
                    }
                }
            }
        }

        #endregion
    }

    interface IGridInstanceProvider
    {
        public GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent);
    }

    public class ObjectsGridInstanceProvider : IGridInstanceProvider
    {
        public GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            return GameObject.Instantiate(gameObject, position, rotation, parent);
        }
    }

    public class GizmoGridInstanceProvider : IGridInstanceProvider
    {
        private float resolution;
        private float height = .1f;

        public GizmoGridInstanceProvider(Color gizmoColor, float resolution, float height)
        {
            Gizmos.color = gizmoColor;
            this.resolution = resolution;
            this.height = height;
        }

        public GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            Gizmos.DrawWireCube(position, new Vector3(resolution, height, resolution));

            return null;
        }
    }
}