using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;
    [SerializeField]
    private string RemoteLayerName = "RemotePlayer";

    [SerializeField]
    GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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
                sceneCamera.gameObject.SetActive(false);

            // Create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure player UI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("No UI");
            ui.SetController(GetComponent<PlayerController>());
        }
        GetComponent<Player>().SetupPlayer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    // Assgign RemotePlayer layer
    void AssignRemoteLayer ()
    {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
    }

    // Disable un-needed components
    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
            componentsToDisable[i].enabled = false;
    }

    void OnDisable ()
    {
        // Destroy playerUI
        Destroy(playerUIInstance);

        // Reenable scene camera
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        GameManager.DeRegisterPlayer(transform.name);
    }
}
