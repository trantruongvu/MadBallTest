using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponController))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    private PlayerWeapon currentWeapon;
    private WeaponController weaponController;

    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject muzzlePoint;

    void Start ()
    {
        weaponController = GetComponent<WeaponController>();
    }

    void Update ()
    {
        currentWeapon = weaponController.GetCurrentWeapon();

        // Check firerate
        if (currentWeapon.FireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.FireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

    }

    // Called on server when a Player shoots
    [Command]
    void CmdOnShoot ()
    {
        RpcDoShootEffect();
    }

    // Is called on all Clients to display Shoot Effect
    [ClientRpc]
    void RpcDoShootEffect ()
    {
        //Debug.Log("Player shoot Graphics: " + weaponController.GetCurrentGraphics().name);
        weaponController.GetCurrentGraphics().muzzleFlash.Play();
    }

    // Called on server when a Player hit something
    // take in hit position (point) and hit surface (normal)
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    // Is called on all Clients to display Hit effect
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect =  Instantiate(weaponController.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1f);
    }

    [Client]
    void Shoot ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // Shooting
        CmdOnShoot();

        //Vector3 forward = muzzlePoint.transform.TransformDirection(Vector3.forward) * 10;
        //Debug.DrawRay(muzzlePoint.transform.position, forward, Color.cyan, 2f);

        Ray ray = new Ray (muzzlePoint.transform.position, muzzlePoint.transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                //  Player got shot
                CmdPlayerGotShot(_hit.collider.name, currentWeapon.Damage);
            }

            // Hitting something -> Call onHit on server
            CmdOnHit(_hit.point, _hit.normal);
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
