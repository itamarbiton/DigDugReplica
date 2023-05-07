using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private GridGenerator3D gridGenerator;
    [SerializeField] private GameObject leftWallPrefab;
    [SerializeField] private GameObject topLeftCornerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateRoom(leftWallPrefab, topLeftCornerPrefab, gridGenerator.columns, gridGenerator.rows);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CreateRoom(GameObject leftWallPrefab, GameObject topLeftCornerPrefab, float halfWidth, float halfHeight) {
        // Calculate half dimensions for positioning
        float width = halfWidth * 2f;
        float height = halfHeight * 2f;

        // Create left wall
        GameObject leftWall = Instantiate(leftWallPrefab, new Vector3(-(halfWidth + 1), 0f, 0f), Quaternion.Euler(0f, 180f, 0f));
        leftWall.transform.localScale = new Vector3(1f, 1f, halfHeight - 1);

        // Create top wall
        GameObject topWall = Instantiate(leftWallPrefab, new Vector3(0f, 0f, halfHeight + 1), Quaternion.Euler(0f, 270f, 0f));
        topWall.transform.localScale = new Vector3(1f, 1f, halfWidth - 1);

        // Create right wall
        GameObject rightWall = Instantiate(leftWallPrefab, new Vector3(halfWidth + 1, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
        rightWall.transform.localScale = new Vector3(1f, 1f, halfHeight - 1);

        // Create bottom wall
        GameObject bottomWall = Instantiate(leftWallPrefab, new Vector3(0f, 0f, -(halfHeight + 1)), Quaternion.Euler(0f, 90f, 0f));
        bottomWall.transform.localScale = new Vector3(1f, 1f, halfWidth - 1);

        // Create top-left corner
        GameObject topLeftCorner = Instantiate(topLeftCornerPrefab, new Vector3(-(halfWidth + 1), 0f, (halfHeight + 1)), Quaternion.Euler(0f, 180f, 0f));
        topLeftCorner.transform.localScale = Vector3.one;

        // Create top-right corner
        GameObject topRightCorner = Instantiate(topLeftCornerPrefab, new Vector3((halfWidth + 1), 0f, (halfHeight + 1)), Quaternion.Euler(0f, 270f, 0f));
        topRightCorner.transform.localScale = Vector3.one;

        // Create bottom-left corner
        GameObject bottomLeftCorner = Instantiate(topLeftCornerPrefab, new Vector3(-(halfWidth + 1), 0f, -(halfHeight + 1)), Quaternion.Euler(0f, 90f, 0f));
        bottomLeftCorner.transform.localScale = Vector3.one;

        // Create bottom-right corner
        GameObject bottomRightCorner = Instantiate(topLeftCornerPrefab, new Vector3((halfWidth + 1), 0f, -(halfHeight + 1)), Quaternion.Euler(0f, 0f, 0f));
        bottomRightCorner.transform.localScale = Vector3.one;
    }

}
