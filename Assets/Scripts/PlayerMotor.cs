﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    private Rigidbody rgbd;
    private Vector3 velocity = Vector3.zero;

    void Start ()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    // Run every physics iteration
    void FixedUpdate ()
    {
        PerformMovement();
    }

    // Get movement
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    private void PerformMovement ()
    {
        if (velocity != Vector3.zero)
        {
            rgbd.MovePosition(rgbd.position + velocity * Time.fixedDeltaTime);
        }
    }
}