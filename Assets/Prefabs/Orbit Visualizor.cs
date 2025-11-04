using System.Collections;
using System.Collections.Generic;

//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitVisualizor : MonoBehaviour
{
    [SerializeField]
    private int numSteps;
    [SerializeField]
    private Dictionary<GameObject, List<Vector3>> planetOrbitPoints;
    [SerializeField]
    private List<Vector2> initialVelocities;
    [SerializeField]
    private Transform planetTransform;
    private float planetMass;
    [SerializeField]
    private GameObject[] planets;
    [SerializeField]
    PlanetGravity[] planetGravities;

    [SerializeField]
    private float repeatRate;
    [SerializeField]
    private Transform sunPosition;
    private void Start()
    {
        if (!Application.IsPlaying(gameObject))
        {
            planets = GameObject.FindGameObjectsWithTag("Orbiting Body");
            planetGravities = FindObjectsOfType<PlanetGravity>();
        }
        PredictOrbits(planets);
        foreach(var (key, value) in planetOrbitPoints)
        {
            DrawCurrentSteps(value, key);
        }
    }
    private void OnValidate()
    {
        PredictOrbits(planets); 
    }
   

    // Future note: Place all calculations in a different script so that other objects can use the same calculation method.
    private void PredictOrbits(GameObject[] planets)
    {
        float timeStep = 0.1f;

        planetOrbitPoints = new Dictionary<GameObject, List<Vector3>>();
        Dictionary<GameObject, Vector2> totalForces = new Dictionary<GameObject, Vector2>();
        Dictionary<GameObject, Vector2> velocities = new Dictionary<GameObject, Vector2>();
        Dictionary<GameObject, Vector2> positions = new Dictionary<GameObject, Vector2>();


        int p = 0; // Temporary shit
        foreach (GameObject planet in planets)
        {
            planetOrbitPoints.Add(planet, new List<Vector3>());
            totalForces.Add(planet, new Vector2(0, 0));
            GravityEffected components = planet.GetComponent<GravityEffected>();

            components.initialVelocity = initialVelocities[p];
            velocities.Add(planet, initialVelocities[p]);
            positions.Add(planet, planet.transform.position);
            p++;
        }

        for (int i = 0; i < numSteps; i++)
        {
            Dictionary<GameObject, Vector2> tempPositions = new Dictionary<GameObject, Vector2>();

            foreach (GameObject planet in positions.Keys)
            {
                tempPositions.Add(planet, positions[planet]);
            }
            foreach (GameObject planet in planets)
            {
                Vector2 totalForce = Vector2.zero;

                foreach (PlanetGravity planetGravity in planetGravities)
                {
                    PlanetGravity thisPlanetGravity = planet.GetComponent<PlanetGravity>();
                    GravityEffected thisPlanetEffect = planet.GetComponent<GravityEffected>();

                    if (thisPlanetGravity != planetGravity)
                    {
                        if (!tempPositions.ContainsKey(planetGravity.gameObject))
                        {
                            Vector2 forceOfGravity = planetGravity.GetGravityForce(tempPositions[planet], thisPlanetEffect.mass);
                            totalForce += forceOfGravity;

                        }
                        else
                        {
                            Vector2 forceOfGravity = planetGravity.GetFutureGravityForce(tempPositions[planet], thisPlanetEffect.mass, tempPositions[planetGravity.gameObject]);
                            totalForce += forceOfGravity;
                        }

                    }
                }

                totalForces[planet] = totalForce;
            }
            foreach (GameObject planet in planets)
            {
                GravityEffected thisPlanetEffect = planet.GetComponent<GravityEffected>();

                // Apply velocity
                velocities[planet] += totalForces[planet] * timeStep;
                positions[planet] += velocities[planet] * timeStep;

                planetOrbitPoints[planet].Add(positions[planet]);
            }
        }

    }

    private void DrawCurrentSteps(List<Vector3> points, GameObject planet)
    {
        // Add drawing of orbit points for one list at a time
        LineRenderer line = planet.GetComponent<LineRenderer>();

        if (line != null)
        {
            // Kinda works but it draws a small slice for some reason

            for(int i = 0; i < points.Count; i++)
            {
                points[i] = new Vector3(points[i].x, points[i].y, planet.transform.position.z);
            }

            line.useWorldSpace = true;
            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
            

            // Adjust how often the dots repeat | From ChatGPT
            //float tilingAmount = totalDistance * repeatRate;

            //line.material.mainTextureScale = new Vector2(tilingAmount, 1);
        }
        else
        {
            Debug.Log("Planet no have LineRenderer");
        }
        
    }
    private void DrawX(Vector3 position)
    {

    }

    
    public void OnDrawGizmos()
    {
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
    }
}
