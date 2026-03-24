using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitVisualizor : MonoBehaviour
{
    [SerializeField]
    private int numSteps;
    private GravityBody[] bodies;
    private GravityPlanet[] sources;
    private void Start()
    {
        if (!Application.IsPlaying(gameObject))
        {
            
        }
        PredictOrbits();
        /*
        foreach(var (key, value) in planetOrbitPoints)
        {
            DrawCurrentSteps(value, key);
        }
        */
    }
    private void OnValidate()
    {
        List<List<Vector2>> paths = PredictOrbits();
        DrawCurrentSteps(paths);
    }
   

    // Now I gotta have this work with RB behaviours or i guess velocity affecting because
    // changing positions in fixedUpdate might be janky because without RB there would be no
    // collisions which means the player wont be able to stand on planets n shi, so gotta make
    // a script that changed the velocity of the RB between predictions.
    private List<List<Vector2>> PredictOrbits()
    {
        bodies = FindObjectsOfType<GravityBody>();

        List<BodyState> bodyStates = new List<BodyState>();

        // Convert to structs, from GPT
        foreach (var b in bodies)
        {
            bodyStates.Add(b.GetState());
        }

        return new List<List<Vector2>>(TrajectorySimulator.Predict(bodyStates, numSteps, Time.fixedDeltaTime));
    }

    private void DrawCurrentSteps(List<List<Vector2>> paths)
    {
        foreach (var body in bodies)
        {
            
            LineRenderer line = body.GetComponent<LineRenderer>();
            if (line == null)
            {
                line = body.gameObject.AddComponent<LineRenderer>();
            }
            line.widthMultiplier = 2f;
            line.useWorldSpace = true;
            line.positionCount = 0;
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            var path = paths[i];
            var body = bodies[i];

            LineRenderer line = body.GetComponent<LineRenderer>();
            if (line == null) continue;

            line.positionCount = path.Count;

            for (int j = 0; j < path.Count; j++)
            {
                line.SetPosition(j, new Vector3(path[j].x, path[j].y, 0));
            }
        }
    }
    private void DrawX(Vector3 position)
    {

    }

    
    public void OnDrawGizmos()
    {
        /*
        if(planetOrbitPoints != null)
        {
            Gizmos.color = Color.white;
            foreach(GameObject planet in planetOrbitPoints.Keys)
            {
                for(int i = 0; i < planetOrbitPoints[planet].Count - 1; i++)
                {
                    Vector3 from = new Vector3(planetOrbitPoints[planet][i].x, planetOrbitPoints[planet][i].y, 0);
                    Vector3 to = new Vector3(planetOrbitPoints[planet][i+1].x, planetOrbitPoints[planet][i+1].y, 0);
                    Gizmos.DrawLine(from, to);
                }
            }
        }  
        */
    }
}
