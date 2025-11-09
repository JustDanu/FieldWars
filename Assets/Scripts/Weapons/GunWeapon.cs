using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public Transform firePoint;
    private float nextFire;

    public override void TriggerPress()
    {
        TryShoot();
    }

    public override void UpdateWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }
    
    private void TryShoot()
    {
        if (Time.time < nextFire) return;

        nextFire = Time.time + 1f / gunData.fireRate;
        var proj = Instantiate(gunData.projectileData.prefab, firePoint.position, firePoint.rotation);

        proj.GetComponent<ProjectileBase>().Initialize(gunData.projectileData);
    }
}
