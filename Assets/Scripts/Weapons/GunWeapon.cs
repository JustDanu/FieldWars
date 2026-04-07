using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Tanks;
using Unity.VisualScripting;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    public Transform firePoint;
    private float nextFire;
    private Vector2 dir;
    private bool projectileSpawned;
    private List<BodyState> projectileDraw = new List<BodyState>();

    void Start()
    {
        
    }
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

        // A little funky but should work for this gun of one projectile.
        BodyState projectilePrediction = new BodyState
        {
            position = firePoint.position,
            velocity = dir * gunData.projectileData.speed,
            mass = gunData.projectileData.mass,
            affectsGravity = false,
            affectedByGravity = true

        };
        projectileDraw.Add(projectilePrediction);

        projectileSpawned = true;
    }

    void UpdateAiming()
    {
        if (!projectileSpawned) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for(int i = 0; i < projectileDraw.Count; i++)
        {
            projectileDraw[i] = new BodyState
            {
                position = firePoint.position,
                velocity = dir * gunData.projectileData.speed,
                mass = gunData.projectileData.mass,
                affectsGravity = false,
                affectedByGravity = true

            };;
        }
        OrbitVisualizor.Instance.drawNextSteps(1000, projectileDraw);
        
    }

    void ReleaseShot()
    {
        if (!projectileSpawned) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        handler.CMDReleaseProj(projectileDraw[0].velocity, projectileDraw[0].position, angle);

        projectileSpawned = false;
    }
}
