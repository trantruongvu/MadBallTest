using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private GameObject muzzlePoint;
    [SerializeField]
    private LayerMask mask;


    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    void Start ()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        // Check firerate
        if (currentWeapon.FireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
                Shoot();
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.FireRate);
            else if (Input.GetButtonUp("Fire1"))
                CancelInvoke("Shoot"); 
        }
    }

    [Client]
    void Shoot()
    {
        Vector3 forward = muzzlePoint.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(muzzlePoint.transform.position, forward, Color.magenta, 2f);

        RaycastHit _hit;
        if (Physics.Raycast(muzzlePoint.transform.position, muzzlePoint.transform.forward, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.Damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot (string _ID, int _damage)
    {
        Debug.Log(_ID + " got shot.");
        Player _player = GameManager.GetPlayer(_ID);
        _player.RpcTakeDamage(_damage);
    }
}
