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

    [Client]
    void Shoot ()
    {
        Vector3 forward = muzzlePoint.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(muzzlePoint.transform.position, forward, Color.cyan, 2f);

        Ray ray = new Ray (muzzlePoint.transform.position, muzzlePoint.transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerGotShot(_hit.collider.name, currentWeapon.Damage);
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
