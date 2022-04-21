using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracers : MonoBehaviour {

    private int randomIndex;

    void Start()
    {
        GetRandomTrap();
    }

    void GetRandomTrap()
    {
        randomIndex = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == randomIndex)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
