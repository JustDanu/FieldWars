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
        //Debug.Log("" + dir);
        rb.AddForce(dir * projData.speed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        
    }
}
