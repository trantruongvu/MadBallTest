using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    void Start ()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon (PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = Instantiate(currentWeapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);
        
        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if (currentGraphics == null)
            Debug.LogError(_weaponIns.name + "has no weapon graphics.");
    }

    public PlayerWeapon GetCurrentWeapon ()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }
}
