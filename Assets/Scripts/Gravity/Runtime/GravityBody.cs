using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public float mass;
    public Vector2 velocity;

    public BodyState GetState()
    {
        return new BodyState(transform.position, velocity, mass);
    }
}
