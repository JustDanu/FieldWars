using UnityEngine;
public interface IWeapon
{
    void Initialize(WeaponHandler handler);
    void TriggerPress(Vector2 dir);
    void TriggerRelease();
    void UpdateWeapon();
}