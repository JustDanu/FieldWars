using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public static class TrajectorySimulator
{
    public static List<List<Vector2>> Predict(List<BodyState> bodies, List<GravitySource> gravSourcess, int steps, float dt)
    {
        // Empty list that will be filled with lists of vectors to draw.
        List<List<Vector2>> paths = new();

        for(int i = 0; i < bodies.Count; i++)
        {
            paths.Add(new List<Vector2>());
        }

        for(int step = 0; step < steps; step++)
        {
            SimulateStep(bodies, gravSourcess, dt);

            for(int i = 0; i < bodies.Count; i++)
            {
                paths[i].Add(bodies[i].position);
            }
        }
        return paths;
    }

    public static void SimulateStep(List<BodyState> bodies, List<GravitySource> gravitySources, float dt)
    {
        // Apply gravity
        for (int i = 0; i < bodies.Count; i++)
        {
            var body = bodies[i];

            Vector2 totalAccel = Vector2.zero;

            foreach (var src in gravitySources)
            {
                Vector2 dir = src.position - body.position;
                float distSq = dir.sqrMagnitude + 0.01f;

                float accel = src.mass / distSq;
                totalAccel += dir.normalized * accel;
            }

            body.velocity += totalAccel * dt;

            bodies[i] = body; 
        }

        // Move bodies
        for (int i = 0; i < bodies.Count; i++)
        {
            var body = bodies[i];

            body.position += body.velocity * dt;

            bodies[i] = body;
        }
    }
}
