using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ProjectileBase : NetworkBehaviour
{
    protected ProjectileData projData;
    public Rigidbody2D rb;
    public Vector2 dir;
    private SpriteRenderer sprite;
    public virtual void Initialize(ProjectileData projectileData)
    {
        projData = projectileData;
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void updateVelocity(Vector2 direction)
    {
        rb.velocity = direction * projData.speed;
    }

    public void enableObject()
    {
        rb.simulated = true;
        sprite.enabled = true;
        Destroy(this, 20f);
    }


    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isServer) return;

        PlayerHealth health = col.GetComponent<PlayerHealth>();
        if (health != null)
        {
            Debug.Log("Took Damage");
            health.TakeDamage(projData.damage);
        }

        NetworkServer.Destroy(gameObject);
    }
}
