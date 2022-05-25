using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    [SerializeField]
    private GameObject muzzlePoint;
    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;
    bool isShooting;
    // Stop between shots
    bool isAbleToShoot;
    float currentStopFire;

    void Start()
    {
        //if (isLocalPlayer)
        //    gameObject.GetComponent<PlayerController>().getAudioListener().enabled = true;
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            currentWeapon = weaponManager.GetCurrentWeapon();

            isShooting = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
            CancelInvoke("Shoot");
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if (!isShooting)
            return;

        // Check firerate
        if (currentStopFire < 0f)
        {
            Shoot();
            currentStopFire = currentWeapon.FireRate;
        }
        else
            currentStopFire -= Time.fixedDeltaTime;
    }

    // call on Server when player Shoot
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // call on all Client to do Shoot Effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        weaponManager.GetCurrentGraphics().PlayAudioShoot();
        //StartCoroutine(RenderMuzzleFlash());
    }

    // call on all Client to do Shoot Effect
    [ClientRpc]
    void RpcDoBoosterEffect()
    {

        //StartCoroutine(RenderMuzzleFlash());
    }

    // call on Server when Hit something
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal, Vector3 _direction, float _distance)
    {
        RpcDoHitEffect(_pos, _normal, _direction, _distance);
    }

    // call on all Client to do Hit Effect
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal, Vector3 _direction, float _distance)
    {
        weaponManager.GetCurrentGraphics().RenderTracer(_direction * _distance);

        if (_pos == Vector3.zero)
            return;
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1f);
    }

    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
            return;

        float shotDistance = currentWeapon.Range;

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

            // Update tầm bắn
            shotDistance = _hit.distance;

            //// When hit, call the OnHit on server
            //CmdOnHit(_hit.point, _hit.normal, ray.direction, shotDistance);
        }

        // When hit, call the OnHit on server
        CmdOnHit(_hit.point, _hit.normal, ray.direction, shotDistance);

        // Render tracer
        weaponManager.GetCurrentGraphics().RenderTracer(ray.direction * shotDistance);
    }

    // When player got Shot
    [Command]
    void CmdPlayerShot(string _ID, int _damage)
    {
        Debug.Log(_ID + " got shot.");
        Player _player = GameManager.GetPlayer(_ID);
        _player.RpcTakeDamage(_damage);
    }

    // Render Spark
    IEnumerator RenderMuzzleFlash()
    {
        GameObject _tracerPrefab = weaponManager.GetCurrentGraphics().bulletTracer;
        _tracerPrefab.transform.position = new Vector3(0, 0, 2.5f);
        _tracerPrefab.transform.localScale = new Vector3(15, 2, 0);
        GameObject _shootEffect = Instantiate(_tracerPrefab, muzzlePoint.transform);
        yield return null;
        Destroy(_shootEffect);
    }
}
