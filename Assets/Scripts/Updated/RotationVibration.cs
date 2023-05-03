using UnityEngine;

public class RotationVibration : MonoBehaviour
{
    [SerializeField] private float frequency = 1f; // Noise frequency
    [SerializeField] private float amplitude = 15f; // Vibration amplitude in degrees
    [SerializeField] private float speed = 1f; // Vibration speed

    private float initialRotationZ;
    private float noiseOffset;

    private void Awake()
    {
        initialRotationZ = transform.localEulerAngles.z;
        noiseOffset = Random.Range(0f, 1000f); // Random offset to make each object's vibration unique
    }

    private void Update()
    {
        VibrateRotation();
    }

    private void VibrateRotation()
    {
        float noise = (Mathf.PerlinNoise(Time.time * speed + noiseOffset, 0) * 2 - 1) * amplitude;
        float currentRotationZ = initialRotationZ + noise;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, currentRotationZ);
    }
}