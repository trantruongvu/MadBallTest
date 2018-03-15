using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;
    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
        // Get player ID
        RegisterPlayer();
    }

    // Register player
    void RegisterPlayer ()
    {
        string _ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
        rb.useGravity = true;
    }

    // Assign RemotePlayer Layer
    void AssignRemoteLayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    // Disable components
    void DisableComponents ()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    // Re-Enable Scene Camera
    void OnDisable ()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
