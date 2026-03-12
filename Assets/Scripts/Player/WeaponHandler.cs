using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponSlot;
    public WeaponBase currentWeapon;
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
        currentWeapon?.UpdateWeapon();

        RotateWeapon();
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
