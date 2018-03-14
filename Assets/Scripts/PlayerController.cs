using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 7f;

    private Quaternion targetRotation;
    private float rotationSpeed = 1000f;

    private PlayerMotor playerMotor;
    private CharacterController controller;
    private Camera cam;

    void Start ()
    {
        playerMotor = GetComponent<PlayerMotor>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update ()
    {

        //Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Vector3 motion = input;

        //if (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)
        //{ motion *= 0.7f; }
        //else
        //{ motion *= 1f; }

        //if (Input.GetButton("Run"))
        //{ motion *= runSpeed; }
        //else
        //{ motion *= walkSpeed; }

        //motion += Vector3.up * -8;
        //controller.Move(motion * Time.deltaTime);

        /////Calculate input for movement
        float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;

        // Apply movement
        if (Input.GetButton("Run"))
        {
            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * runSpeed;
            playerMotor.Move(_velocity);
        }
        else // walk
        {
            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
            playerMotor.Move(_velocity);
        }

        // Make camera stop rotate
        ///// Calculate input for Rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));

        Vector3 currentPos = mousePos - new Vector3(transform.position.x, 0f, transform.position.z);
        targetRotation = Quaternion.LookRotation(currentPos);
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,
            targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);


    }

}
