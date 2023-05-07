using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Updated
{
    public class GridManager : MonoBehaviour
    {
        public static event Action<GridManager> GridDidLoad;
        public static event Action AllGrassBladesCut;
        
        public Vector3[,] GridData { get; private set; }
        public GameObject[,] GridGameObjects { get; private set; }

        [SerializeField] public int rows;
        [SerializeField] public int columns;
        [SerializeField] private float spacing;
        [SerializeField] private List<GameObject> gridPrefabs;

        private int grassBladesCount;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #region Initialization

        public void InitializeGrid()
        {
            GridData = GenerateGridData();
            
            InstantiateGrid(GridData);
            
            GridDidLoad?.Invoke(this);
        }

        #endregion

        #region Event Handling

        private void SubscribeEvents()
        {
            GrassBladeController.GrassBladeDidCut += OnGrassBladeCut;
        }

        private void UnsubscribeEvents()
        {
            GrassBladeController.GrassBladeDidCut -= OnGrassBladeCut;
        }

        private void OnGrassBladeCut()
        {
            grassBladesCount -= 1;
        
            if (grassBladesCount <= 0)
            {
                AllGrassBladesCut?.Invoke();
            }
        }

        #endregion

        #region Private Implementation Details

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

        #endregion
    }
}