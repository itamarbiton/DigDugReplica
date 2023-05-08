using UnityEngine;
using Updated;

public class GameManager : MonoBehaviour
{
    private GridManager gridManager;
    private EnemyManager enemyManager;
    private PlayerManager playerManager;

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

        gridManager = GetComponent<GridManager>();
        enemyManager = GetComponent<EnemyManager>();
        playerManager = GetComponent<PlayerManager>();

        gridManager.SetConfiguration(LevelDataProvider.CurrentLevel.gridConfig);
        enemyManager.SetEnemyPercent(LevelDataProvider.CurrentLevel.enemyPercent);

        gridManager.InitializeGrid();
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

    #region Game Handling

    private void HandleMovement()
    {
        if (playerManager != null)
        {
            HandlePlayerKeyboardNavigation();
            
            HandlePlayerGamepadNavigation();

            playerManager.HandleMovement();
        }

        if (enemyManager != null)
        {
            enemyManager.HandleMovement();
        }
    }

    private void HandlePhysics()
    {
        if (playerManager != null)
        {
            playerManager.HandlePhysics();
        }
    }

    private void HandlePlayerKeyboardNavigation()
    {
        KeyCode current = KeyCode.None;
        if (Input.GetKeyDown(KeyCode.RightArrow)) current = KeyCode.RightArrow;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) current = KeyCode.LeftArrow;
        else if (Input.GetKeyDown(KeyCode.UpArrow)) current = KeyCode.UpArrow;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) current = KeyCode.DownArrow;
        if (current != KeyCode.None)
        {
            playerManager.ChangeDirectionOnArrow(current);
        }
    }

    private void HandlePlayerGamepadNavigation()
    {
        if (!isGameRunning && TouchJoystick.IsTouching && TouchJoystick.CurrentDirection != TouchJoystick.Direction.None) return;

        if (playerManager != null)
        {
            var keyCode = KeyCode.None;
            switch (TouchJoystick.CurrentDirection)
            {
                case TouchJoystick.Direction.Up:
                    keyCode = KeyCode.UpArrow;
                    break;
                case TouchJoystick.Direction.Down:
                    keyCode = KeyCode.DownArrow;
                    break;
                case TouchJoystick.Direction.Left:
                    keyCode = KeyCode.LeftArrow;
                    break;
                case TouchJoystick.Direction.Right:
                    keyCode = KeyCode.RightArrow;
                    break;
            }

            playerManager.ChangeDirectionOnArrow(keyCode);
        }
    }

    #endregion

    #region Event Handling

    private void SubscribeEvents()
    {
        InstructionsController.InstructionsFinished += OnFinishedInstructions;
        TapSideDetector.SideTapped += OnSideTapped;
    }

    private void UnsubscribeEvents()
    {
        InstructionsController.InstructionsFinished -= OnFinishedInstructions;
        TapSideDetector.SideTapped -= OnSideTapped;
    }

    private void OnFinishedInstructions()
    {
        isGameRunning = true;
    }

    private void OnSideTapped(TapSideDetector.ScreenSide side)
    {
        if (!isGameRunning) return;

        if (playerManager != null)
        {
            playerManager.ChangeDirectionOnSideTap(side);
        }
    }

    #endregion

    #region Private Implementation Details

    private void UpdateConfigurations()
    {
        enemyManager.SetEnemyPercent(LevelDataProvider.CurrentLevel.enemyPercent);
    }

    #endregion
}