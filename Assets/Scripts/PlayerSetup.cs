using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;

    void Start ()
    {
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
        // Setup player
        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        // Register player
        GameController.RegisterPlayer(_netID, _player);
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
            Cursor.visible = true;
            sceneCamera.gameObject.SetActive(true);
        }
        // Register player
        //GameController.DeRegisterPlayer(transform.name);
    }

}
