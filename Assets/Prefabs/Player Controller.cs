using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float acceleration;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Transform nearestPlanet;
    public GravityEffected gravityEffects;
    private void Start()
    {
        gravityEffects = this.GetComponent<GravityEffected>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(isLocalPlayer)
        {
            float xMove = Input.GetAxis("Horizontal");
            float yMove = Input.GetAxis("Vertical");

            moveInput = new Vector2(xMove, yMove).normalized;
        }
    }
    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            //rb.AddForce(moveInput * acceleration, ForceMode2D.Force);

            // Apperently this prefents drift buildup (Legacy thingy)
            /**
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed; // Keep the momentum and keep going!
            }
            */


            // Player Ground Check
            PlanetGravity nearestPlanet = gravityEffects.GetClosetPlanet();
            float dist = Vector2.Distance(transform.position, nearestPlanet.transform.position);

            // Gravity direction
            Vector2 gravityDir = (nearestPlanet.transform.position - transform.position).normalized;
            SimpleDebugDraw.Arrow(transform.position, gravityDir * 2f, Color.green);

            // Movement
            float horizontalMovement = moveInput.x;
            Vector2 tangent = new Vector2(-gravityDir.y, -gravityDir.x);
            SimpleDebugDraw.Arrow(transform.position, tangent * horizontalMovement * moveSpeed, Color.green);
            rb.AddForce(tangent * horizontalMovement * moveSpeed);
        }
    }
}
