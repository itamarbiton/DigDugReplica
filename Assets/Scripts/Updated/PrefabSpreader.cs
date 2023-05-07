using UnityEngine;

public class PrefabSpreader : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count;
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] public int seed;

    private Vector3[] positions;

    private void Start()
    {
        GeneratePositions(seed);
        SpreadPrefabs(width, height, prefab, count);
    }

    public void GeneratePositions(int seed)
    {
        positions = new Vector3[count];
        Random.InitState(seed);
        for (int i = 0; i < count; i++)
        {
            positions[i] = GeneratePositionOutsideRectangle(width, height);
        }
    }

    public void SpreadPrefabs(float width, float height, GameObject prefab, int count)
    {
        Transform parentTransform = transform;

        for (int i = 0; i < count; i++)
        {
            Vector3 position = positions[i];
            Instantiate(prefab, position, Quaternion.identity, parentTransform);
        }
    }

    private Vector3 GeneratePositionOutsideRectangle(float width, float height)
    {
        float halfWidth = width / 2;
        float halfHeight = height / 2;
        float x, z;

        bool onVerticalEdge = Random.value > 0.5f;
        if (onVerticalEdge)
        {
            x = Random.value > 0.5f ? -halfWidth : halfWidth;
            z = Random.Range(-halfHeight, halfHeight);
        }
        else
        {
            x = Random.Range(-halfWidth, halfWidth);
            z = Random.value > 0.5f ? -halfHeight : halfHeight;
        }

        return new Vector3(x, 0, z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        if (positions == null) return;
        else
        {
            foreach (Vector3 position in positions)
            {
                Gizmos.DrawSphere(position, 0.5f);
            }
        }

        // Draw the rectangle
        Vector3 topLeft = new Vector3(-width / 2, 0, height / 2);
        Vector3 topRight = new Vector3(width / 2, 0, height / 2);
        Vector3 bottomLeft = new Vector3(-width / 2, 0, -height / 2);
        Vector3 bottomRight = new Vector3(width / 2, 0, -height / 2);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
