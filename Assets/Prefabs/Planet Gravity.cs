using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlanetGravity : MonoBehaviour
{
    [SerializeField]
    private float gravityStrength;

    public Vector2 GetGravityForce(Vector2 objectPosition, float objectMass)
    {
        Vector2 direction = GetGravityDirection(objectPosition, transform.position);
        float distance = direction.magnitude;
        float forceMagnitude = (gravityStrength * objectMass) / (distance * distance);
        return direction.normalized * forceMagnitude;
    }

    public Vector2 GetFutureGravityForce(Vector2 objectPosition, float objectMass, Vector2 thisObjectPosition)
    {
        Vector2 direction = GetGravityDirection(objectPosition, thisObjectPosition);
        float distance = direction.magnitude;
        float forceMagnitude = (gravityStrength * objectMass) / (distance * distance);
        return direction.normalized * forceMagnitude;
    }

    public Vector2 GetGravityDirection(Vector2 objectPosition, Vector2 thisObjectPosition)
    {
        return thisObjectPosition - objectPosition;
    }
}
