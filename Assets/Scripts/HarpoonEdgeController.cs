using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonEdgeController : MonoBehaviour
{
    public BoxCollider2D innerCollider { get; private set; }

    private void Start()
    {
        innerCollider = GetComponent<BoxCollider2D>();
    }

    public void EnableHarpoon()
    {
        innerCollider.enabled = true;
    }

    public void DisableHarpoon()
    {
        innerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("[TEST]: harpoon edge collided: " + collision);
        EventManager.HarpoonHit(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.HarpoonDetached();
    }
}
