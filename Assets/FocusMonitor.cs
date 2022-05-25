using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusMonitor : MonoBehaviour
{
    [SerializeField] AudioListener audioListener;
    private void Start()
    {
        audioListener.enabled = Application.isFocused;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        audioListener.enabled = hasFocus;
    }

    //private void FixedUpdate()
    //{
    //    audioListener.enabled = Application.isFocused;

    //    if (Application.isFocused)
    //        transform.localPosition = pos;
    //    else
    //        transform.position = new Vector3(0, 10, -10);
    //}
}
