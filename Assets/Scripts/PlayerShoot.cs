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
    private LineRenderer shotTracer;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject muzzlePoint;
    [SerializeField]
    private Transform spawnTracer;

    void Start ()
    {
        weaponController = GetComponent<WeaponController>();
        shotTracer = GetComponent<LineRenderer>();
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
    void CmdOnShoot (Vector3 _hitpoint)
    {
        RpcDoShootEffect(_hitpoint);
    }

    // Is called on all Clients to display Shoot Effect
    [ClientRpc]
    void RpcDoShootEffect (Vector3 _hitPoint)
    {
        weaponController.GetCurrentGraphics().muzzleFlash.Play();
        // Render tracer
        StartCoroutine("RenderTracer", _hitPoint);
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

        

        Ray ray = new Ray (muzzlePoint.transform.position, muzzlePoint.transform.forward);
        RaycastHit _hit;
        float shotDistance = 15f;

        if (Physics.Raycast(ray, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                //  Player got shot
                CmdPlayerGotShot(_hit.collider.name, currentWeapon.Damage);
            }
            shotDistance = _hit.distance;
            // Hitting something -> Call onHit on server
            CmdOnHit(_hit.point, _hit.normal);
        }

        // Shooting
        CmdOnShoot(ray.direction * shotDistance);
    }


    IEnumerator RenderTracer(Vector3 shotPos)
    {
        shotTracer.enabled = true;
        shotTracer.SetPosition(0, spawnTracer.position);
        shotTracer.SetPosition(1, spawnTracer.position + shotPos);
        yield return null;
        shotTracer.enabled = false;
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
