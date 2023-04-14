using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonEdgeController : MonoBehaviour
{
    private BoxCollider2D collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public void EnableHarpoon()
    {
        collider.enabled = true;
    }

    public void DisableHarpoon()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventManager.HarpoonHit(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.HarpoonDetached();
    }
}
