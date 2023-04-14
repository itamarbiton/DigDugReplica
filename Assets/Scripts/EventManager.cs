using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<GameObject> OnHarpoonHit;
    public static event Action OnHarpoonDetached;

    public static void HarpoonHit(GameObject hitObject)
    {
        if (OnHarpoonHit != null)
        {
            OnHarpoonHit(hitObject);
        }
    }

    public static void HarpoonDetached()
    {
        if (OnHarpoonDetached != null)
        {
            OnHarpoonDetached();
        }
    }
}
