using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct GravitySource
{
    public Vector2 position;
    public float mass;

    public GravitySource(Vector2 pos, float m)
    {
        position = pos;
        mass = m;
    }
}

