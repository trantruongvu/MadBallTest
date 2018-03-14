using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float walkSpeed = 5f;

    [SerializeField]
    private float runSpeed = 7f;

    [SerializeField]
    public Camera cam;

    private Quaternion targetRotation;
    private float rotationSpeed = 1500f;
    private CharacterController controller;

    void Start ()
    {
        controller = GetComponent<CharacterController>();
        //cam = Camera.main;
    }

    void Update ()
    {
        InputKeyBoard();
        InputMouse();
    }

    void InputKeyBoard ()
    {
        // Player movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        Vector3 motion = input;

        // In case of Pythago
        if (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)
        {
            motion *= 0.7f;
        }
        else
        {
            motion *= 1f;
        }

        // Run
        if (Input.GetButton("Run"))
        {
            motion *= runSpeed;
        }
        else // walk
        {
            motion *= walkSpeed;
        }

        motion += Vector3.up * -8;
        controller.Move(motion * Time.deltaTime);
    }

    void InputMouse ()
    {
        ///// Player Rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));

        Vector3 currentPos = mousePos - new Vector3(transform.position.x, 0f, transform.position.z);
        targetRotation = Quaternion.LookRotation(currentPos);
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
            targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
    }
}
