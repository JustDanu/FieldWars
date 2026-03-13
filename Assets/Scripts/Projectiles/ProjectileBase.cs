using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected ProjectileData projData;
    public Rigidbody2D rb;
    public Vector2 dir;
    public virtual void Initialize(ProjectileData projectileData)
    {
        projData = projectileData;
        rb = this.GetComponent<Rigidbody2D>();

        Destroy(gameObject, projData.lifeTime);
        SimpleDebugDraw.Arrow(transform.position, dir, Color.red);
        rb.velocity = dir * projData.speed;
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
