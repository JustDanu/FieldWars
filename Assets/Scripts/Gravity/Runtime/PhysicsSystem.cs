using System.Collections.Generic;
using UnityEngine;

public class GravityPhysicsSystem : MonoBehaviour
{
    public float dt;

    private GravityBody[] bodies;

    void Start()
    {
        Debug.Log("Start Phys");
        //bodies = FindObjectsOfType<GravityBody>();
    }

    void FixedUpdate()
    {
        bodies = FindObjectsOfType<GravityBody>();
        List<BodyState> states = new List<BodyState>();

        foreach (var body in bodies)
        {
            states.Add(body.GetState());
        }
            
        TrajectorySimulator.SimulateStep(states, dt);

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].ApplyState(states[i]);
        }
    }
}