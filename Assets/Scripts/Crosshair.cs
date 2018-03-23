﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D crosshairImage;
    Vector2 mousePos;

    void OnGUI()
    {
        Cursor.visible = false;
        float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (crosshairImage.width / 2);
        float yMin = (Screen.height - Input.mousePosition.y) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }

    void Update()
    {
        mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void OnDisable ()
    {
        Cursor.visible = true;
    }
}
