using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponSlot;
    public WeaponBase currentWeapon;

    //Test
    public GunData starterGun;

    void Start()
    {
        EquipWeapon(starterGun);
    }

    public void EquipWeapon(GunData newGunData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - weaponSlot.position;
        SimpleDebugDraw.Arrow(weaponSlot.position, dir, Color.magenta);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
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
