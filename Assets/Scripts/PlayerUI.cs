﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform thrusterFuelFill;

    private PlayerController controller;

    void Update ()
    {
        SetFuelAmount(controller.GetFuelAmount());
    }

    public void SetController (PlayerController _controller)
    {
        controller = _controller;
    }

    void SetFuelAmount (float _amount)
    {
        thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
    }

}
