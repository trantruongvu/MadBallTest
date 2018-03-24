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

    private Rigidbody rigidbd;
    private Quaternion targetRotation;
    private float rotationSpeed = 2000f;

    void Start ()
    {
        rigidbd = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        InputKeyBoard();
        InputMouse();
    }

    void InputKeyBoard ()
    {
        Vector3 _velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Apply movement
        if (Input.GetButton("Run"))
            rigidbd.velocity = _velocity *= runSpeed;
        else // walk
            rigidbd.velocity = _velocity *= walkSpeed;
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
