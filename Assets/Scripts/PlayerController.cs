using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public GameObject dugTile;
    [SerializeField] private GameObject harpoon;

    public float speed = 1f;
    public float dugTileThreshold = .33f;

    public Vector3[,] gridPositions;
    public float gridSpacing;
    public float turnDistance = 2f;

    public Vector3 lastPosition;

    private SpriteRenderer spriteRenderer;
    private Vector2 facingDir;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        harpoon = transform.Find("Weapon").gameObject;
        if (harpoon == null)
        {
            Debug.Log("[ERROR] failed to find dig dug weapon!");
        }
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        if (hInput != 0 || vInput != 0)
        {
            facingDir = new Vector2(hInput, vInput).normalized;
            OrientPlayer(facingDir);
        }

        if (gridPositions != null && gridSpacing > 0)
        {
            MovePlayerOnGrid(gridPositions, hInput, vInput);
            SpawnDugTileIfNeeded();
        }
    }

    bool CanPlayerMoveX(float threshold)
    {
        for (int i = 0; i < gridPositions.GetLength(0); i++)
        {
            var position = gridPositions[i, 0];
            var distance = Mathf.Abs(transform.position.y - position.y);

            if (distance < threshold)
            {
                return true;
            }
        }

        return false;
    }

    bool CanPlayerMoveY(float threshold)
    {
        for (int i = 0; i < gridPositions.GetLength(1); i++)
        {
            var position = gridPositions[0, i];
            var distance = Mathf.Abs(transform.position.x - position.x);

            if (distance < threshold)
            {
                return true;
            }
        }

        return false;
    }

    public void MovePlayerOnGrid(Vector3[,] gridPositions, float hInput, float vInput)
    {
        if (hInput != 0)
        {
            var canMoveX = CanPlayerMoveX(.1f);
            if (!canMoveX)
            {
                var closestY = GetClosestY(transform.position, gridPositions, .1f);

                hInput = 0;
                vInput = (closestY > 0) ? 1 : -1;
            }
        }

        if (vInput != 0)
        {
            var canMoveY = CanPlayerMoveY(.1f);
            if (!canMoveY)
            {
                var closestX = GetClosestX(transform.position, gridPositions, .1f);

                vInput = 0;
                hInput = (closestX > 0) ? 1 : -1;
            }
        }

        Vector3 positionDelta = new Vector3(
            hInput * speed * Time.deltaTime,
            vInput * speed * Time.deltaTime,
            0f
        );

        Vector3 playerPosition = transform.position;
        Vector3 newPosition = playerPosition + positionDelta;
        Vector3 clampedNewPosition = levelGenerator.ClampPositionToBounds(newPosition);
        transform.position = clampedNewPosition;
    }

    public void OrientPlayer(Vector2 facingDirection)
    {
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        spriteRenderer.flipY = facingDirection.x < 0;
    }

    public float GetClosestX(Vector3 position, Vector3[,] gridPositions, float threshold)
    {
        float closestX = 0f;
        float closestDistance = Mathf.Infinity;

        for (int j = 0; j < gridPositions.GetLength(1); j++)
        {
            Vector3 gridPosition = gridPositions[0, j];

            // ignore points below the threshold distance
            if (Vector3.Distance(position, gridPosition) < threshold)
            {
                continue;
            }

            float distance = Mathf.Abs(gridPosition.x - position.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestX = gridPosition.x;
            }
        }

        return closestX;
    }

    public float GetClosestY(Vector3 position, Vector3[,] gridPositions, float threshold)
    {
        float closestY = 0f;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < gridPositions.GetLength(0); i++)
        {
            Vector3 gridPosition = gridPositions[i, 0];

            // ignore points below the threshold distance
            if (Vector3.Distance(position, gridPosition) < threshold)
            {
                continue;
            }

            float distance = Mathf.Abs(gridPosition.y - position.y);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestY = gridPosition.y;
            }
        }

        return closestY;
    }

    public void SpawnDugTileIfNeeded()
    {
        if (lastPosition != null)
        {
            if (Vector3.Distance(transform.position, lastPosition) > dugTileThreshold)
            {
                SpawnDugTile(transform.position);
            }
        }

        else
        {
            SpawnDugTile(transform.position);
        }
    }

    public void SpawnDugTile(Vector3 position)
    {
        levelGenerator.Dig(position);
        lastPosition = position;
    }
}
