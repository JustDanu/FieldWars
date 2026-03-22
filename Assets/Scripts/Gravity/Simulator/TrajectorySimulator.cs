using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class TrajectorySimulator
{
    public static List<List<Vector2>> Predict(List<BodyState> bodies, int steps, float dt)
    {
        // Empty list that will be filled with lists of vectors to draw.
        List<List<Vector2>> paths = new();

        for(int i = 0; i < bodies.Count; i++)
        {
            paths.Add(new List<Vector2>());
        }

        for(int step = 0; step < steps; step++)
        {
            SimulateStep(bodies, dt);

            for(int i = 0; i < bodies.Count; i++)
            {
                paths[i].Add(bodies[i].position);
            }
        }
        return paths;
    }

    public static void SimulateStep(List<BodyState> bodies, float dt)
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            var body = bodies[i];

            if (!body.affectedByGravity) continue; // Skip if not affected by gravity

            Vector2 totalAccel = Vector2.zero;

            for (int j = 0; j < bodies.Count; j++)
            {
                if (i == j) continue; // skip self

                var other = bodies[j];

                if (!other.affectsGravity) continue; // skip objects that dont affect gravity

                Vector2 dir = other.position - body.position;
                float distSq = Mathf.Max(dir.sqrMagnitude, 0.5f);

                float accel = other.mass / distSq;

                totalAccel += dir.normalized * accel;
            }

            body.velocity += totalAccel * dt;
            bodies[i] = body;
        }

        for (int i = 0; i < bodies.Count; i++)
        {
            var body = bodies[i];
            body.position += body.velocity * dt;
            bodies[i] = body;
        }
    }
}
