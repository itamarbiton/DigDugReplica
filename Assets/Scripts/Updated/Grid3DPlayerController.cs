using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid3DPlayerController : MonoBehaviour
{
    public static event Action PlayerDidDie;
    
    public bool IsWalking { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public Vector3[,] GridData;

    [FormerlySerializedAs("smokeParticlesystem")] [SerializeField] private ParticleSystem smokeParticleSystem;
    [SerializeField] private AudioClip grassPopAudioClip;
    [SerializeField] private AudioClip explosionAudioClip;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float speed = 3f;
    [SerializeField] private GameObject explosionPrefab;

    private Vector3 direction = Vector3.right;
    private Vector2 gridPosition = new(0, 0);

    private Quaternion targetRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
    private Quaternion lastCurrentRotation = Quaternion.LookRotation(Vector3.right, Vector3.up);

    private float rotationDuration = .12f;
    private float rotationTime;

    private AudioSource audioSource;
    private float audioPitch = 1f;
    private float grassPopCooldown;
    private bool shouldSmoke;
    private bool didWin;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GridGenerator3D.AllGrassBladesCut += OnAllGrassBladesCut;
    }

    private void OnAllGrassBladesCut()
    {
        didWin = true;
    }

    private void OnDestroy()
    {
        GridGenerator3D.AllGrassBladesCut -= OnAllGrassBladesCut;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            smokeParticleSystem.Play();
        }

        if (grassPopCooldown > 0)
        {
            grassPopCooldown -= Time.deltaTime;
            grassPopCooldown = Mathf.Max(0, grassPopCooldown);
        }
        
        if (audioPitch >= 1f)
        {
            audioPitch -= 1f * Time.deltaTime;
            audioPitch = Mathf.Max(1f, audioPitch);
        }

        audioSource.pitch = audioPitch;
    }

    private void FixedUpdate()
    {
        if (rotationTime <= rotationDuration)
        {
            float t = curve.Evaluate(rotationTime / rotationDuration);
            transform.rotation = Quaternion.Slerp(lastCurrentRotation, targetRotation, t);
            rotationTime += Time.fixedDeltaTime;
        }
        
    }

    public void HandleInput()
    {
        if (!IsAlive) return;

        SwipeDirection swipeDirection = SwipeDetector.Instance.GetLastSwipeData().Direction;
        
        if (Input.GetKeyDown(KeyCode.DownArrow) || swipeDirection == SwipeDirection.Down)
        {
            direction = Vector3.back;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || swipeDirection == SwipeDirection.Left)
        {
            direction = Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || swipeDirection == SwipeDirection.Right)
        {
            direction = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || swipeDirection == SwipeDirection.Up)
        {
            direction = Vector3.forward;
        }
    }

    public void HandleMovement()
    {
        if (!IsAlive) return;

        if (GridData == null || GridData.Length == 0) return;

        if (IsWalking) return;

        Vector2 targetGridPosition = new Vector2(
            gridPosition.x + direction.z, 
            gridPosition.y + direction.x);
        
        if (targetGridPosition.x >= GridData.GetLength(0) || targetGridPosition.x < 0)
            return;

        if (targetGridPosition.y >= GridData.GetLength(1) || targetGridPosition.y < 0)
            return;

        Walk(targetGridPosition);
    }

    public void Walk(Vector2 gridDest)
    {
        StartCoroutine(WalkCoroutine(gridDest));
    }

    private IEnumerator WalkCoroutine(Vector2 gridDest)
    {
        var position = transform.position;

        Vector3 dest = GridData[(int)gridDest.x, (int)gridDest.y];
        dest.y = position.y;

        targetRotation = Quaternion.LookRotation((dest - position).normalized, Vector3.up);
        lastCurrentRotation = transform.rotation;
        rotationTime = 0f;
        
        IsWalking = true;
        
        float targetDistance = Vector3.Distance(position, dest);
        
        while (targetDistance > 0.1f)
        {
            transform.position += (dest - transform.position).normalized * (speed * Time.deltaTime);
            targetDistance = Vector3.Distance(transform.position, dest);
            yield return null;
        }

        gridPosition = gridDest;

        IsWalking = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        var moleController = other.gameObject.GetComponentInParent<MoleController>();
        if (!didWin && moleController != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            CinemachineShake.Instance.ShakeCamera(5f, 1f);

            GameObject explosionSource = new GameObject("Explosion Source");
            AudioSource explosionAudioSource = explosionSource.AddComponent<AudioSource>();
            explosionAudioSource.PlayOneShot(explosionAudioClip);
            Destroy(explosionSource, 10f);

            IsAlive = false;
            Destroy(gameObject);
            
            PlayerDidDie?.Invoke();
        }
        else if (other.gameObject.CompareTag("GrassBlade"))
        {
            audioPitch += .01f;
            if (grassPopCooldown <= 0)
            {
                grassPopCooldown = .05f;
                audioSource.Play();
            }
        }
    }
}
