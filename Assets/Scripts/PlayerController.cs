using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 7f;

    [SerializeField]
    private float fuelBurnSpeed = 1f;
    [SerializeField]
    private float fuelRegenSpeed = 0.25f;
    private float fuelAmount = 1f;

    [SerializeField]
    public Camera cam;

    private Rigidbody rigidbd;
    private Quaternion targetRotation;
    private float rotationSpeed = 1500f;
    //private CharacterController controller;

    // Getter fuel Amount
    public float GetFuelAmount ()
    {
        return fuelAmount;
    }

    void Start ()
    {
        //controller = GetComponent<CharacterController>();
        rigidbd = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        InputKeyBoard();
        InputMouse();
    }

    void InputKeyBoard ()
    {
        //// Player movement
        //Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //Vector3 motion = input;
        //// In case of Pythago
        //if (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)
        //{
        //    motion *= 0.7f;
        //}
        //else
        //{
        //    motion *= 1f;
        //}
        //// Run -> Use FUEL
        //if (Input.GetButton("Run") && fuelAmount > 0)
        //{
        //    fuelAmount -= fuelBurnSpeed * Time.deltaTime;
        //    if (fuelAmount >= 0.01f)
        //    {
        //        motion *= runSpeed;
        //    }
        //}
        //else // walk
        //{
        //    fuelAmount += fuelRegenSpeed * Time.deltaTime;
        //    motion *= walkSpeed;
        //}
        //fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);
        //motion += Vector3.up * -8;
        //controller.Move(motion * Time.deltaTime);
        Vector3 _velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetButton("Run"))
        {
            fuelAmount -= fuelBurnSpeed * Time.deltaTime;
            if (fuelAmount >= 0.01f)
            {
                rigidbd.velocity = _velocity *= runSpeed;
            }
        }
        else
        {
            fuelAmount += fuelRegenSpeed * Time.deltaTime;
            rigidbd.velocity = _velocity *= walkSpeed;
        }
        fuelAmount = Mathf.Clamp(fuelAmount, 0f, 1f);
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
