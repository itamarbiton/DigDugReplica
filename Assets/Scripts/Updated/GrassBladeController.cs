using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBladeController : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
