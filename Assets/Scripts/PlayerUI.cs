using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    RectTransform fuelFill;

    private PlayerController controller;

    public void SetController (PlayerController _controller)
    {
        controller = _controller;
    }

    void SetFuelAmount (float _amount)
    {
        fuelFill.localScale = new Vector3(1f, _amount, 1f);
    }
     
    void Update ()
    {
        SetFuelAmount(controller.GetFuelAmount());
    }
}
