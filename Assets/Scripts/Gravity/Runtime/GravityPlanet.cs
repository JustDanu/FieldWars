using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanet : MonoBehaviour
{
    public float mass;

    public GravitySource GetSource()
    {
        return new GravitySource(transform.position, mass);
    }
}
