using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private float destroyTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
