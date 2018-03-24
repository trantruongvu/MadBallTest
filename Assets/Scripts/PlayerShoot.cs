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

    // call on Server when player Shoot
    [Command]
    void CmdOnShoot ()
    {
        RpcDoShootEffect();
    }

    // call on all Client to do Shoot Effect
    [ClientRpc]
    void RpcDoShootEffect ()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        StartCoroutine(RenderTracer());
    }

    // call on Server when Hit something
    [Command]
    void CmdOnHit (Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal); 
    }

    // call on all Client to do Hit Effect
    [ClientRpc]
    void RpcDoHitEffect (Vector3 _pos, Vector3 _normal)
    {
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
            return;

        // When Shoot, call the OnShoot on server
        CmdOnShoot();

        Ray ray = new Ray(muzzlePoint.transform.position, muzzlePoint.transform.forward);
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, currentWeapon.Range, mask))
        {
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_hit.collider.name, currentWeapon.Damage);
            }

            // When hit, call the OnHit on server
            CmdOnHit(_hit.point, _hit.normal);
        }
    }

    // When player got Shot
    [Command]
    void CmdPlayerShot (string _ID, int _damage)
    {
        Debug.Log(_ID + " got shot.");
        Player _player = GameManager.GetPlayer(_ID);
        _player.RpcTakeDamage(_damage);
    }

    // Render bullet tracer
    IEnumerator RenderTracer()
    {
        GameObject _tracerPrefab = weaponManager.GetCurrentGraphics().bulletTracer;
        _tracerPrefab.transform.position = new Vector3(0, 0, 2.5f);
        _tracerPrefab.transform.localScale = new Vector3(15, 2, 0);
        GameObject _shootEffect = Instantiate(_tracerPrefab, muzzlePoint.transform);
        yield return null;
        Destroy(_shootEffect);
    }
}
