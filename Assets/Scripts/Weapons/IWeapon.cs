
public interface IWeapon
{
    void Initialize(WeaponHandler handler);
    void TriggerPress();
    void TriggerRelease();
    void UpdateWeapon();
}