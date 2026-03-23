using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct BodyState
{
    public int id;
    public Vector2 position;
    public Vector2 velocity;
    public float mass;
    public bool affectsGravity;
    public bool affectedByGravity;
    public float size;

    public BodyState(int ident, Vector2 pos, Vector2 vel, float m, bool affects, bool affected, float r)
    {
        id = ident;
        position = pos;
        velocity = vel;
        mass = m;
        affectsGravity = affects;
        affectedByGravity = affected;
        size = r;
    }
}
