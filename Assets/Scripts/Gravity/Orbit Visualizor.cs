using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitVisualizor : MonoBehaviour
{
    [SerializeField]
    private int numSteps;
    [SerializeField]
    private TrajectoryDrawer drawerPrefab;
    private List<TrajectoryDrawer> drawers = new List<TrajectoryDrawer>();
    private GravityBody[] bodies;
    private GravityPlanet[] sources;

    public static OrbitVisualizor Instance;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void EnsureDrawerCount(int count)
    {
        while (drawers.Count < count)
        {
            var drawer = Instantiate(drawerPrefab, transform);
            drawers.Add(drawer);
        }
    }

    private void OnValidate()
    {
        //List<List<Vector2>> paths = PredictOrbits(numSteps, null);
        //DrawCurrentSteps(paths);
    }

    public void drawNextSteps(int steps, List<BodyState> extraBodies = null)
    {
        List<List<Vector2>> paths = PredictOrbits(steps, extraBodies);
        DrawCurrentSteps(paths);
    }
   

    // Now I gotta have this work with RB behaviours or i guess velocity affecting because
    // changing positions in fixedUpdate might be janky because without RB there would be no
    // collisions which means the player wont be able to stand on planets n shi, so gotta make
    // a script that changed the velocity of the RB between predictions.
    private List<List<Vector2>> PredictOrbits(int steps, List<BodyState> extraBodies)
    {
        bodies = FindObjectsOfType<GravityBody>();

        List<BodyState> bodyStates = new List<BodyState>();

        // Convert to structs, from GPT
        foreach (var b in bodies)
        {
            bodyStates.Add(b.GetState());
        }

        // Add extra predicted bodies from GPT
        if (extraBodies != null)
        {
            bodyStates.AddRange(extraBodies);
        }

        return new List<List<Vector2>>(TrajectorySimulator.Predict(bodyStates, steps, Time.fixedDeltaTime));
    }

    private void DrawCurrentSteps(List<List<Vector2>> paths)
    {
        EnsureDrawerCount(paths.Count);

        for(int i = 0; i < paths.Count; i++)
        {
            drawers[i].Draw(paths[i]);
        }

        for(int i = paths.Count; i < drawers.Count; i++)
        {
            drawers[i].Clear();
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
