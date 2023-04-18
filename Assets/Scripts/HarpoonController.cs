using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{
    [SerializeField] private float detachDuration = 1f;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private float radius = .3f;

    private float detachTimer = -1f;

    private HarpoonEdgeController edgeController;
    private Animator harpoonAnimator;
    private GameObject attachedObject;

    private bool isAttached
    {
        get { return attachedObject != null; }
    }

    private void Start()
    {
        harpoonAnimator = GetComponent<Animator>();
        edgeController = gameObject.GetComponentInChildren<HarpoonEdgeController>();

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Update()
    {
        HandleInput();
        HandleDetach();

        List<Vector3> dugPositions = levelGenerator.dugTilePositions;
        Vector3 edgePos = edgeController.transform.position;

        bool validPos = dugPositions.Exists(point =>
        {
            float distance = Vector3.Distance(point, edgePos);

            if (distance <= radius)
            {
                return true;
            }

            return false;
        });

        if (!validPos)
        {
            Debug.Log("[TEST]: failed to find point!");
            WithdrawHarpoon();
        }
    }

    private void WithdrawHarpoon()
    {
        if (!edgeController.innerCollider.enabled) return;

        edgeController.DisableHarpoon();

        harpoonAnimator.SetTrigger("WithdrawHarpoon");

        Debug.Log("[TEST]: harpoon withdrawn");

        if (attachedObject != null && attachedObject.GetComponent<EnemyController>() is IInflatable inflatable)
        {
            inflatable.StopInflating();
        }

        attachedObject = null;
        detachTimer = 0;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isAttached)
            {
                Debug.Log("[TEST]: not attached!");
                edgeController.EnableHarpoon();
                harpoonAnimator.SetTrigger("ShootHarpoon");
            }
            else if (attachedObject != null)
            {
                Debug.Log("[TEST]: attached!");
                AttemptInflation(attachedObject);
            }
            else
            {
                Debug.Log("[ERROR: attached but no attached object, error!");
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isAttached)
            {
                WithdrawHarpoon();
            }
        }
    }

    private void AttemptInflation(GameObject target)
    {
        if (attachedObject.GetComponent<EnemyController>() is IInflatable inflatable)
        {
            inflatable.Inflate();
            detachTimer = 0f;
        }
    }

    private void HandleDetach()
    {
        if (!isAttached) return;

        detachTimer += Time.deltaTime;

        if (detachTimer >= detachDuration)
        {
            WithdrawHarpoon();
        }
    }

    private void OnHarpoonHit(GameObject hitObject)
    {
        EnemyController enemyController = hitObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            harpoonAnimator.speed = 0;
            attachedObject = hitObject;
        }
    }

    private void OnHarpoonDetached()
    {
        if (attachedObject != null && attachedObject.GetComponent<EnemyController>() != null)
        {
            harpoonAnimator.speed = 1;
        }
    }

    private void SubscribeEvents()
    {
        EventManager.OnHarpoonHit += OnHarpoonHit;
        EventManager.OnHarpoonDetached += OnHarpoonDetached;
    }

    private void UnsubscribeEvents()
    {
        EventManager.OnHarpoonHit -= OnHarpoonHit;
        EventManager.OnHarpoonDetached -= OnHarpoonDetached;
    }
}
