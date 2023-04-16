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

    public float turnDistance = 2f;

    public Vector3 lastPosition;

    private SpriteRenderer spriteRenderer;
    private Vector2 facingDir;
    private Animator animator;

    private bool isDead = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        harpoon = transform.Find("Weapon").gameObject;
        if (harpoon == null)
        {
            Debug.Log("[ERROR] failed to find dig dug weapon!");
        }
    }

    void Update()
    {
        if (isDead) return;

        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        if (hInput != 0 || vInput != 0)
        {
            facingDir = new Vector2(hInput, vInput).normalized;
            OrientPlayer(facingDir);
        }

        if (levelGenerator.gridPositions != null && levelGenerator.spacing > 0)
        {
            MovePlayerOnGrid(levelGenerator.gridPositions, hInput, vInput);
            SpawnDugTileIfNeeded();
        }
    }

    public void MovePlayerOnGrid(Vector3[,] gridPositions, float hInput, float vInput)
    {
        if (hInput != 0)
        {
            var canMoveX = MovementUtilities.CanPlayerMoveX(gridPositions, transform.position, .1f);
            if (!canMoveX)
            {
                var closestY = MovementUtilities.GetClosestY(transform.position, gridPositions, .1f);

                hInput = 0;
                vInput = (closestY > 0) ? 1 : -1;
            }
        }

        if (vInput != 0)
        {
            var canMoveY = MovementUtilities.CanPlayerMoveY(gridPositions, transform.position, .1f);
            if (!canMoveY)
            {
                var closestX = MovementUtilities.GetClosestX(transform.position, gridPositions, .1f);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() is EnemyController enemyController)
        {
            Die();
        }
    }

    private void Die()
    {
        harpoon.SetActive(false);
        isDead = true;
        animator.SetTrigger("Die");
    }
}
