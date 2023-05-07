using System;
using UnityEngine;

namespace Updated
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;

        private Grid3DPlayerController player;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void OnDisable()
        {
            UnsusbscribeEvents();
        }

        #region Configuration

        public void HandleMovement()
        {
            player.HandleMovement();
        }

        public void HandlePhysics()
        {
            player.HandleRotation();
            player.HandleDrive();
        }

        public void ChangeDirectionOnSideTap(TapSideDetector.ScreenSide tappedSide)
        {
            player.ChangeDirection(tappedSide);
        }

        public void ChangeDirectionOnArrow(KeyCode keyCode)
        {
            player.ChangeDirection(keyCode);
        }

        #endregion

        #region Event Handling

        private void SubscribeEvents()
        {
            GridManager.GridDidLoad += OnGridDidLoad;
        }

        private void UnsusbscribeEvents()
        {
            GridManager.GridDidLoad -= OnGridDidLoad;
        }

        private void OnGridDidLoad(GridManager gridGenerator)
        {
            InstantiatePlayer(gridGenerator.GridData);
        }

        #endregion

        #region Private Implementation Details

        private void InstantiatePlayer(Vector3[,] gridData)
        {
            Vector2Int startCoords = new Vector2Int(0, 0);

            Vector3 startPos3 = new Vector3(
                gridData[startCoords.x, startCoords.y].x,
                0,
                gridData[startCoords.x, startCoords.y].z
            );

            Quaternion startRot = Quaternion.Euler(Vector3.up * 90);
            
            GameObject playerGameObject = Instantiate(playerPrefab, startPos3, startRot);

            player = playerGameObject.GetComponent<Grid3DPlayerController>();
            player.GridData = gridData;
        }

        #endregion
    }
}