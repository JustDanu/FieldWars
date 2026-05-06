using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Projectile")]
public class ProjectileData : ScriptableObject
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public int damage = 10;
    public float mass = 1f;
    public GameObject prefab;
}