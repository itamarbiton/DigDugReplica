using System.Collections;
using UnityEngine;

public class Grid3DPlayerController : MonoBehaviour
{
    public bool IsWalking { get; private set; }
    public Vector3[,] GridData;

    [SerializeField] private float speed = 3f;

    private Vector3 direction = Vector3.right;
    private Vector2 gridPosition = new(0, 0);

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Vector3.back;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Vector3.forward;
        }
    }

    public void HandleMovement()
    {
        if (GridData == null || GridData.Length == 0) return;

        if (IsWalking) return;

        Vector2 targetGridPosition = new Vector2(
            gridPosition.x + direction.z, 
            gridPosition.y + direction.x);
        
        if (targetGridPosition.x >= GridData.GetLength(0) || targetGridPosition.x < 0)
            return;

        if (targetGridPosition.y >= GridData.GetLength(1) || targetGridPosition.y < 0)
            return;

        Walk(targetGridPosition);
    }

    public void Walk(Vector2 gridDest)
    {
        StartCoroutine(WalkCoroutine(gridDest));
    }

    private IEnumerator WalkCoroutine(Vector2 gridDest)
    {
        var position = transform.position;
        
        Vector3 dest = GridData[(int)gridDest.x, (int)gridDest.y];
        dest.y = position.y;
        
        IsWalking = true;
        
        float targetDistance = Vector3.Distance(position, dest);
        
        while (targetDistance > 0.1f)
        {
            transform.position += (dest - transform.position).normalized * (speed * Time.deltaTime);
            targetDistance = Vector3.Distance(transform.position, dest);
            yield return null;
        }

        gridPosition = gridDest;
        
        Debug.Log("updated grid position: " + gridPosition);

        IsWalking = false;
    }
}
