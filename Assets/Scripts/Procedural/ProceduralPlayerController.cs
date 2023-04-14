using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProceduralPlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform centerBone;

    private float speed = 3f;

    [SerializeField]
    private float regularSpeed = 3f;

    [SerializeField]
    private float runSpeed = 6f;

    [SerializeField]
    private float rotateSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Debug.Log(Gamepad.all[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        if (Gamepad.all.Count > 0)
        {
            Gamepad gamepad = Gamepad.all[0];

            vInput = gamepad.leftStick.y.ReadValue();
            hInput = gamepad.rightStick.x.ReadValue();

            speed = gamepad.rightTrigger.IsPressed() ? runSpeed : regularSpeed;
        }

        if (vInput != 0)
        {
            Vector3 forward = centerBone.right.normalized * vInput * speed * Time.deltaTime;
            centerBone.position += forward;
        }

        if (hInput != 0)
        {
            Vector3 degrees = new Vector3(0f, 0f, -hInput * rotateSpeed * Time.deltaTime); 
            centerBone.Rotate(degrees);
        }
    }
}
