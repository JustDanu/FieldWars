using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;
    public bool affectsGravity;
    public bool affectedByGravity;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    public BodyState GetState()
    {
        return new BodyState(gameObject.GetInstanceID(), rb.position, rb.velocity,
        mass, affectsGravity, affectedByGravity, transform.localScale.magnitude/2f);
    }

    public void ApplyState(BodyState state)
    {
        rb.velocity = state.velocity;
    }
}
