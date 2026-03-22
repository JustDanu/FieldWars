using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct BodyState
{
    public Vector2 position;
    public Vector2 velocity;
    public float mass;

    public BodyState(Vector2 pos, Vector2 vel, float m)
    {
        position = pos;
        velocity = vel;
        mass = m;
    }
}
