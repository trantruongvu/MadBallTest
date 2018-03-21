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
    private Camera cam;

    private Vector3 inputMovement;

    void Start ()
    {
        playerMotor = GetComponent<PlayerMotor>();
        cam = Camera.main;
    }

    void Update ()
    {

        //Calculate input for movement
        Vector3 _velocity = new Vector3(Input.GetAxisRaw
            ("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // Apply movement
        if (Input.GetButton("Run"))
            playerMotor.Move(_velocity *= runSpeed);
        else // walk
            playerMotor.Move(_velocity *= walkSpeed);

        // Calculate input for Rotation
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Apply Rotation
        if (Physics.Raycast(ray, out hit))
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
    }

}
