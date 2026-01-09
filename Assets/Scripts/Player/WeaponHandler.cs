using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponSlot;
    public WeaponBase currentWeapon;
    private Camera cam;

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
        // Fun stuff to work on here
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 weaponScreenPos = cam.WorldToScreenPoint(weaponSlot.position);

        Vector2 dir = mouseScreenPos - weaponScreenPos;
        SimpleDebugDraw.Arrow(weaponScreenPos, dir, Color.magenta);

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        currentWeapon.transform.rotation = Quaternion.Euler(0, 0, -(angle+90f));
    }

    public void OnFirePressed()
    {
        currentWeapon?.TriggerPress();
    }

    public void OnFireRelease()
    {
        currentWeapon?.TriggerRelease();
    }
}
