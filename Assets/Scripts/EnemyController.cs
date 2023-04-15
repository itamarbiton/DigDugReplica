using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IInflatable
{
    public bool IsDead
    {
        get { return hp <= 0; }
    }

    [SerializeField] private float speed = 1f;
    [SerializeField] private float walkDistanceThreshold = .9f;
    [SerializeField] private List<Sprite> inflationSprites;

    private Queue<Vector3> lastPositions = new Queue<Vector3>(2);
    private Animator animator;
    private int hp = -1;
    private SpriteRenderer spriteRenderer;
    private bool isBeingInflated = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = inflationSprites.Count;
    }

    void Update()
    {

    }

    public void MoveTowardsPosition(Vector3 targetPosition, List<Vector3> dugPositions)
    {
        if (isBeingInflated) return;

        List<Vector3> closePositions = FilterPointsByDistance(transform.position, dugPositions, walkDistanceThreshold);

        if (closePositions.Count == 0) return;

        Vector3 refPosition = Vector3.positiveInfinity;
        if (lastPositions.Count > 1)
            refPosition = lastPositions.Dequeue();

        Vector3 targetPoint = GetClosestPoint(closePositions, targetPosition, refPosition);
        lastPositions.Enqueue(targetPoint);

        Vector3 currentPos = transform.position;

        transform.position = currentPos + (targetPoint - transform.position).normalized * speed * Time.deltaTime;
    }

    public List<Vector3> FilterPointsByDistance(Vector3 position, List<Vector3> points, float threshold)
    {
        List<Vector3> filteredPoints = new List<Vector3>();

        foreach (Vector3 point in points)
        {
            if (Vector3.Distance(position, point) < threshold)
            {
                filteredPoints.Add(point);
            }
        }

        return filteredPoints;
    }

    public Vector3 GetClosestPoint(List<Vector3> points, Vector3 position, Vector3 prevPos)
    {
        Vector3 closestPoint = points[0];
        float closestDistance = Vector3.Distance(position, closestPoint);

        foreach (Vector3 point in points)
        {
            float distance = Vector3.Distance(position, point);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("[TEST]: enemy hit something!" + collision);
        HarpoonEdgeController edgeController = collision.GetComponent<HarpoonEdgeController>();
        Debug.Log("[TEST]: edge controller: " + edgeController);
        if (edgeController != null)
        {
            animator.speed = 0;
            animator.enabled = false;
            isBeingInflated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HarpoonEdgeController edgeController = collision.gameObject.GetComponent<HarpoonEdgeController>();
        if (edgeController != null)
        {
            animator.speed = 1;
            animator.enabled = true;
            isBeingInflated = false;
        }
    }

    public void Inflate()
    {
        hp -= 1;
        spriteRenderer.sprite = inflationSprites[(inflationSprites.Count - 1) - hp];
    }

    public void StopInflating()
    {
        hp = inflationSprites.Count;
    }
}
