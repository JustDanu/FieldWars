using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Projectile")]
public class ProjectileData : ScriptableObject
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public float damage = 10f;
    public float mass = 1f;
    public GameObject prefab;
}