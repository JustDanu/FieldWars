using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Gun")]
public class GunData : ScriptableObject
{
    [Header("Stats")]
    public float damage = 10f;
    public float fireRate = 5f;
    public float projectileSpeed = 20f;
    public int projectilesPerShot = 1;
    public float spread = 0f;

    [Header("Projectile")]
    public ProjectileData projectileData;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("FX")]
    public GameObject muzzleFlash;
    public AudioClip fireSFX;
}