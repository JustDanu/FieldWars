using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected ProjectileData projData;
    public Rigidbody2D rb;
    public virtual void Initialize(ProjectileData projectileData)
    {
        projData = projectileData;
        rb = this.GetComponent<Rigidbody2D>();

        Destroy(gameObject, projData.lifeTime);

        rb.velocity = transform.forward * projData.speed;
    }

    void FixedUpdate()
    {
        
    }
}
