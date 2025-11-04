using UnityEngine;
using Mirror;

[ExecuteAlways]
public class GravityEffected : NetworkBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private PlanetGravity[] planets;
    [SerializeField]
    public float mass;
    [SerializeField]
    public Vector2 initialVelocity;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        planets = FindObjectsOfType<PlanetGravity>();
        rb.velocity = initialVelocity;
    }
    private void FixedUpdate()
    {
        Vector2 totalGravityForce = Vector2.zero;

        foreach(PlanetGravity planet in planets)
        {
            if(this.GetComponent<PlanetGravity>() != planet)
            {
                rb.velocity += planet.GetGravityForce(transform.position, mass) * Time.fixedDeltaTime;
                if(isLocalPlayer)
                {
                    AlignPlayerToClosestGravity();
                }
            }
        }
    }


    private void AlignPlayerToClosestGravity()
    {
        PlanetGravity closestPlanet = GetClosetPlanet();
        Vector2 gravityDirection = closestPlanet.GetGravityDirection(transform.position, closestPlanet.transform.position);
        float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10);
    }

    public PlanetGravity GetClosetPlanet()
    {
        PlanetGravity closestPlanet = null;
        Vector2 difference = Vector2.zero;
        foreach (PlanetGravity planet in planets)
        {
            if (closestPlanet == null)
            {
                closestPlanet = planet;
                difference = transform.position - planet.transform.position;
            }
            else
            {
                difference = transform.position - planet.transform.position;
                Vector2 newDifference = transform.position - closestPlanet.transform.position;

                if (difference.magnitude < newDifference.magnitude)
                {
                    closestPlanet = planet;
                }
            }
        }
        return closestPlanet;
    }
}
