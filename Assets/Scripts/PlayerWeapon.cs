using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon {
    public string Name = "M4A1";
    public int Damage = 10;
    public float Range = 25f;
    public float FireRate = 1f;

    public GameObject graphics;
}
