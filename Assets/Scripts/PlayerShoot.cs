using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private GameObject barrel;

    [SerializeField]
    private LayerMask mask;

    public PlayerWeapon weapon;

    void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot ()
    {
        Vector3 forward = barrel.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(barrel.transform.position, forward, Color.cyan, 2f);

        RaycastHit _hit;
        if (Physics.Raycast(barrel.transform.position, barrel.transform.forward, out _hit, weapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerGotShot(_hit.collider.name, weapon.Damage);
            }
        }
    }

    // Server call Player who got shot
    [Command]
    void CmdPlayerGotShot (string _playerID, int _damage)
    {
        Debug.Log(_playerID + " has been shot.");

        // Player take Damage
        Player _player = GameController.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
