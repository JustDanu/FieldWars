using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IWeapon
{
    protected WeaponHandler handler;
    public GunData gunData;
    public virtual void Initialize(WeaponHandler handler)
    {
        this.handler = handler;
    }

    public virtual void TriggerPress()
    {
        
    }

    public virtual void TriggerRelease()
    {
        
    }

    public virtual void UpdateWeapon()
    {
        
    }
}
