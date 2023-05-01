using System.Collections;
using UnityEngine;

public class GridPlayerController : MonoBehaviour
{
    public bool IsWalking { get; private set; }
    public Vector2[,] GridData;

    [SerializeField] private float speed = 3f;

    private Vector2 direction = Vector2.right;
    private Vector2 gridPosition = new(0, 0);

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = -Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
    }

    public void HandleMovement()
    {
        if (GridData == null || GridData.Length == 0) return;
        
        if (IsWalking) return;

        Vector2 targetGridPosition = new Vector2(gridPosition.x + direction.y, gridPosition.y + direction.x);

        if (targetGridPosition.x >= GridData.GetLength(0) || targetGridPosition.x < 0)
            return;

        if (targetGridPosition.y >= GridData.GetLength(1) || targetGridPosition.y < 0)
            return;

        int something1 = (int)targetGridPosition.x;
        int something2 = (int)targetGridPosition.y;
        
        Debug.Log("something1: " + something1 + ", something2: " + something2);

        Walk(targetGridPosition);
    }

    public void Walk(Vector2 gridDest)
    {
        StartCoroutine(WalkCoroutine(gridDest));
    }

    private IEnumerator WalkCoroutine(Vector2 gridDest)
    {
        Vector2 dest = GridData[(int)gridDest.x, (int)gridDest.y];
        
        Debug.Log("world dest: " + dest);
        
        IsWalking = true;

        var position = transform.position;
        
        float targetDistance = Vector3.Distance(position, new Vector3(dest.x, dest.y, 0));
        Vector3 targetDirection = (new Vector3(dest.x, dest.y, 0) - position).normalized;

        while (targetDistance > 0.05f)
        {
            transform.position += targetDirection * (speed * Time.deltaTime);
            targetDistance = Vector3.Distance(transform.position, new Vector3(dest.x, dest.y, 0));
            yield return null;
        }

        gridPosition = gridDest;

        IsWalking = false;
    }
}
