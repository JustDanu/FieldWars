using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;
    public bool affectsGravity;
    public bool affectedByGravity;

    public BodyState GetState()
    {
        return new BodyState(gameObject.GetInstanceID(), transform.position, velocity, mass, affectsGravity, affectedByGravity);
    }
}
