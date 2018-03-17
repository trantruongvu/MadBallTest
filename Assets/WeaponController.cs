using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    void Start ()
    {
        EquipWeapon(primaryWeapon);
    }

    // Equip WEAPON
    void EquipWeapon (PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        GameObject _weaponIns = Instantiate(currentWeapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
        {
            Debug.LogError(_weaponIns.name + " has no Graphics");
        }

        if (isLocalPlayer)
        {
            _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    // Get current WEAPON
    public PlayerWeapon GetCurrentWeapon ()
    {
        return currentWeapon;
    }

    // Get current GRAPHICS
    public WeaponGraphics GetCurrentGraphics ()
    {
        return currentGraphics;
    }
}
