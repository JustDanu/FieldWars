using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GravityBody : NetworkBehaviour
{
    public float mass;
    public Vector2 velocity;
    public bool affectsGravity;
    public bool affectedByGravity;
    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    public override void OnStartServer()
    {
        Debug.Log("Server started");
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
