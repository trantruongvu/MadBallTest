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
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    GameObject playerUIPrefab;
    public GameObject playerUIInstance;

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
            // We are local player -> disable scene camera
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
            // Create playerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure playerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.Log("NO UI");
            }
            ui.SetController(GetComponent<PlayerController>());

            // Setup player
            GetComponent<Player>().SetupPlayer();
        }
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

    
    void OnDisable ()
    {
        // Destroy playerUI
        Destroy(playerUIInstance);

        // Re-Enable Scene Camera
        if (isLocalPlayer)
        {
            GameController.instance.SetSceneCameraActive(true);
        }
        // Register player
        GameController.DeRegisterPlayer(transform.name);
    }

}
