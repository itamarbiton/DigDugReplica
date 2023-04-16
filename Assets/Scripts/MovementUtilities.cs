using UnityEngine;
using System.Collections;

public class MovementUtilities
{
    public static bool CanPlayerMoveX(Vector3[,] gridPositions, Vector2 currentPos, float threshold)
    {
        for (int i = 0; i < gridPositions.GetLength(0); i++)
        {
            var position = gridPositions[i, 0];
            var distance = Mathf.Abs(currentPos.y - position.y);

            if (distance < threshold)
            {
                return true;
            }
        }

        return false;
    }

    public static bool CanPlayerMoveY(Vector3[,] gridPositions, Vector2 currentPos, float threshold)
    {
        for (int i = 0; i < gridPositions.GetLength(1); i++)
        {
            var position = gridPositions[0, i];
            var distance = Mathf.Abs(currentPos.x - position.x);

            if (distance < threshold)
            {
                return true;
            }
        }

        return false;
    }

    public static float GetClosestX(Vector3 position, Vector3[,] gridPositions, float threshold)
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

    public static float GetClosestY(Vector3 position, Vector3[,] gridPositions, float threshold)
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
}
