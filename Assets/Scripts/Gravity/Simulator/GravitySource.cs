using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct GravitySource
{
    public int id;
    public Vector2 position;
    public float mass;

    public GravitySource(int ident, Vector2 pos, float m)
    {
        id = ident;
        position = pos;
        mass = m;
    }
}

