using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IInflatable
{
    [HideInInspector] public LevelGenerator levelGenerator;

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

    private bool isWalking = false;

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

        if (isWalking) return;

        List<Vector3> closePositions = FilterPointsByDistance(transform.position, dugPositions, walkDistanceThreshold);


        if (closePositions.Count == 0)
        {
            Debug.Log("no close points");
            return;
        }

        Vector3 targetPoint = GetClosestPoint(closePositions, targetPosition, transform.position);

        lastPositions.Enqueue(targetPoint);

        StartCoroutine(WalkToPoint(targetPoint, .1f));

        return;

        Vector3 direction = (targetPoint - transform.position).normalized;
        float hInput = direction.x;
        float vInput = direction.y;

        Debug.DrawRay(transform.position, direction * 3);

        if (hInput != 0)
        {
            if (!MovementUtilities.CanPlayerMoveX(levelGenerator.gridPositions, transform.position, .1f))
            {
                float closestY = MovementUtilities.GetClosestY(transform.position, levelGenerator.gridPositions, .1f);

                hInput = 0;
                vInput = (closestY > 0) ? 1 : -1;
            }
        }

        if (vInput != 0)
        {
            var canMoveY = MovementUtilities.CanPlayerMoveY(levelGenerator.gridPositions, transform.position, .1f);
            if (!canMoveY)
            {
                float closestX = MovementUtilities.GetClosestX(transform.position, levelGenerator.gridPositions, .1f);

                vInput = 0;
                hInput = (closestX > 0) ? 1 : -1;
            }
        }

        Debug.DrawRay(transform.position, new Vector3(hInput, vInput) * 2, Color.cyan);

        Vector3 positionDelta = new Vector3(hInput * speed * Time.deltaTime, vInput * speed * Time.deltaTime, 0f);
        Vector3 currentPos = transform.position;
        Vector3 newPosition = currentPos + positionDelta;
        Vector3 clampedNewPosition = levelGenerator.ClampPositionToBounds(newPosition);

        // transform.position = clampedNewPosition;
        transform.position += direction * speed * Time.deltaTime;
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

    public Vector3 GetClosestPoint(List<Vector3> points, Vector3 targetPos, Vector3 currentPos)
    {
        // initialize closest point and distance
        Vector3 closestPoint = points[0];
        float closestDistance = Vector3.Distance(targetPos, closestPoint);

        // check if there's something better
        foreach (Vector3 point in points)
        {
            float targetDistance = Vector3.Distance(targetPos, point);
            float selfDistance = Vector3.Distance(currentPos, point);

            if (targetDistance < closestDistance && selfDistance > .15f)
            {
                closestDistance = targetDistance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HarpoonEdgeController edgeController = collision.GetComponent<HarpoonEdgeController>();
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

    private IEnumerator WalkToPoint(Vector3 point, float distanceThreshold)
    {
        isWalking = true;

        while (Vector3.Distance(transform.position, point) > distanceThreshold)
        {
            Vector3 dir = (point - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }

        isWalking = false;
    }
}
