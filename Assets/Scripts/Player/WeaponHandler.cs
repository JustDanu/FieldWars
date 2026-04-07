using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    public Transform weaponSlot;
    public WeaponBase currentWeapon;
    private ProjectileBase currentProjectile;
    // Temp
    [SerializeField] private float radius = 1.5f;
    private Camera cam;
    public Vector2 dir;

    //Test
    public GunData starterGun;

    void Start()
    {
        cam = Camera.main;
        EquipWeapon(starterGun);
    }

    public void EquipWeapon(GunData newGunData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        // TODO:
        // Screw this way of rotation, use old method for rotations and account for camera/player rotation.
        weaponSlot.position += newGunData.posOffset;

        //var weaponObj = Instantiate(newGunData.weaponPrefab, newGunData.posOffset + weaponSlot.position, Quaternion.identity);
        var weaponObj = Instantiate(newGunData.weaponPrefab, weaponSlot);
       
        currentWeapon = weaponObj.GetComponent<WeaponBase>();
        currentWeapon.gunData = newGunData;
        currentWeapon.Initialize(this);
    }

    void Update()
    {
        if(!isLocalPlayer) return;
        
        currentWeapon?.UpdateWeapon(dir);

        RotateWeapon();
    }

    [Command]
    public void CMDAimProjectile(GameObject projPrefab, Vector3 position, float rotation)
    {
        var projObj = Instantiate(
            projPrefab,
            position,
            Quaternion.Euler(0, 0, rotation)
        );

        currentProjectile = projObj.GetComponent<ProjectileBase>();
        currentProjectile.Initialize(starterGun.projectileData);
        NetworkServer.Spawn(projObj);
    }

    [Command]
    public void CMDUpdateProjV()
    {
        if(currentProjectile == null) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentProjectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        currentProjectile.updateVelocity(dir);
    }

    [Command]
    public void CMDReleaseProj(Vector2 velocityDirection, Vector2 pos, float rotation)
    {
        var projObj = Instantiate(
            starterGun.projectileData.prefab,
            pos,
            Quaternion.Euler(0, 0, rotation)
        );

        NetworkServer.Spawn(projObj);
        
        currentProjectile = projObj.GetComponent<ProjectileBase>();
        currentProjectile.Initialize(starterGun.projectileData);
        currentProjectile.rb.velocity = velocityDirection;
        
    }

    private void RotateWeapon()
    {
        // Chatgpt implementation using old implem

        dir = GetMouseDirection();
        // 1. Position weapon in a circle around player
        currentWeapon.transform.position = transform.position + (Vector3)(dir * radius);

        // 2. Rotate weapon to face the mouse
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    public void OnFirePressed()
    {
        dir = GetMouseDirection();
        Debug.Log("" + dir);
        currentWeapon?.TriggerPress(dir);
    }

    public void OnFireRelease()
    {
        currentWeapon?.TriggerRelease();
    }

    // Thank you chpt for this cleaner idea
    private Vector2 GetMouseDirection()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // Direction from player center → mouse
        return (mouseWorld - transform.position).normalized;
    }
}
