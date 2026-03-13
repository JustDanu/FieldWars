using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public Transform firePoint;
    private float nextFire;
    private Vector2 dir;

    public override void TriggerPress(Vector2 dir)
    {
        Debug.Log("" + dir);
        this.dir = dir;
        TryShoot();
    }

    public override void UpdateWeapon(Vector2 dir)
    {
        this.dir = dir;
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }
    
    private void TryShoot()
    {
        if (Time.time < nextFire) return;

        nextFire = Time.time + 1f / gunData.fireRate;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var proj = Instantiate(gunData.projectileData.prefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        proj.GetComponent<ProjectileBase>().dir = this.dir;
        proj.GetComponent<ProjectileBase>().Initialize(gunData.projectileData);
        //Debug.Log("" + dir);
        
    }
}
