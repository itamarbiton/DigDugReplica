using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Updated;

public class TutorialRotator : MonoBehaviour
{
    private Quaternion targetRot = Quaternion.identity;
    
    // Start is called before the first frame update
    void Start()
    {
        TapSideDetector.SideTapped += OnSideTapped;
    }

    private void OnDestroy()
    {
        TapSideDetector.SideTapped -= OnSideTapped;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetRot = transform.rotation * Quaternion.Euler(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetRot = transform.rotation * Quaternion.Euler(0, 90, 0);
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 15 * Time.deltaTime);
    }

    private void OnSideTapped(TapSideDetector.ScreenSide side)
    {
        
    }
}
