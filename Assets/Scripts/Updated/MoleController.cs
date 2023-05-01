using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public Vector3[,] GridData;
    public Vector2 GridPosition = new(0, 0);

    [SerializeField] private float speed = 3f;
    [SerializeField] private float waitRange = .5f;

    private Animator animator;

    public bool IsWalking { get; private set; }

    private List<Vector3> availableDirections =
        new List<Vector3> { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private Vector3 direction = Vector3.right;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleMovement()
    {
        if (GridData == null || GridData.Length == 0) return;

        if (IsWalking) return;

        direction = availableDirections[Random.Range(0, availableDirections.Count)];

        Vector2 targetGridPosition = new Vector2(
            GridPosition.x + direction.z, 
            GridPosition.y + direction.x);
        
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

        yield return new WaitForSeconds(Random.Range(0, waitRange));
        
        float targetDistance = Vector3.Distance(position, dest);
        
        while (targetDistance > 0.1f)
        {
            transform.position += (dest - transform.position).normalized * (speed * Time.deltaTime);
            targetDistance = Vector3.Distance(transform.position, dest);
            yield return null;
        }

        GridPosition = gridDest;
        
        animator.SetTrigger("Peek");

        yield return new WaitForSeconds(4);

        IsWalking = false;
    }
}
