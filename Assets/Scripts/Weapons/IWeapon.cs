
public interface IWeapon
{
    void Initialize(WeaponHandler handler);
    void TriggerPress(float angle);
    void TriggerRelease();
    void UpdateWeapon();
}