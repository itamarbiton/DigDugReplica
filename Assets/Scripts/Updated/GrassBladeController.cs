using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBladeController : MonoBehaviour
{
    public static event Action GrassBladeDidCut;

    private void OnCollisionEnter(Collision other)
    {
        GrassBladeDidCut?.Invoke();
        Destroy(gameObject);
    }
}
