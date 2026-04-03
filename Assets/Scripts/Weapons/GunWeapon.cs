using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Tanks;
using Unity.VisualScripting;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public Transform firePoint;
    private float nextFire;
    private Vector2 dir;
    private ProjectileBase currentProjectile;

    public override void TriggerPress(Vector2 dir)
    {
        Debug.Log("" + dir);
        this.dir = dir;
        TryShoot();
    }

    public override void UpdateWeapon(Vector2 dir)
    {
        
        this.dir = dir;
        if (Input.GetMouseButtonDown(0))
        {
            StartAiming();
        }
        if (Input.GetMouseButton(0))
        {
            UpdateAiming();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseShot();
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
    }

    void StartAiming()
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var projObj = Instantiate(
            gunData.projectileData.prefab,
            firePoint.position,
            Quaternion.Euler(0, 0, angle)
        );

        currentProjectile = projObj.GetComponent<ProjectileBase>();

        currentProjectile.dir = dir;
        currentProjectile.Initialize(gunData.projectileData);
    }

    void UpdateAiming()
    {
        if (currentProjectile == null) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentProjectile.transform.position = firePoint.position;
        currentProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        currentProjectile.updateVelocity(dir);
        OrbitVisualizor.Instance.drawNextSteps(1000);
        
    }

    void ReleaseShot()
    {
        if (currentProjectile == null) return;

        currentProjectile.enableObject();

        currentProjectile = null;
    }
}
