using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float acceleration;
    public float jumpForce;
    public float flyAccel;
    public float rotationForce;
    public float planetAlignmentSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public GravityEffected gravityEffects;
    public bool isGrounded;
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
        if (isLocalPlayer)
        {
            // Legacy code

            //rb.AddForce(moveInput * acceleration, ForceMode2D.Force);
            // Apperently this prefents drift buildup
            /**
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed; // Keep the momentum and keep going!
            }
            */


            // Player Ground Check
            PlanetGravity nearestPlanet = gravityEffects.GetClosetPlanet();


            // Gravity direction
            Vector2 gravityDir = (nearestPlanet.transform.position - transform.position).normalized;
            SimpleDebugDraw.Arrow(transform.position, gravityDir * 2f, Color.green);

            // Movement on planet
            if (isGrounded && (Input.GetKey("a") || Input.GetKey("d")))
            {
                Vector2 tangent = gravityDir.Perpendicular2();
                SimpleDebugDraw.Arrow(transform.position, tangent * moveInput.x * moveSpeed, Color.green);
                rb.AddForce(tangent * moveInput.x * moveSpeed);
            }

            // Movement in space left and right (still janky but works, idk about adding forces in the left and right) 
            // IDEA: Maybe cursor will be what rotates and rotate code goes there
            if (!isGrounded && (Input.GetKey("a") || Input.GetKey("d")))
            {
                Vector2 moveDirection = (moveInput.x * transform.right * moveSpeed).normalized;
                SimpleDebugDraw.Arrow(transform.position, transform.right * moveInput.x * moveSpeed, Color.green);
                rb.AddForce(moveInput.x * transform.right * moveSpeed);

                float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationForce);
            }


            // Jumping
            Vector2 distanceFromPlanet = nearestPlanet.transform.position - transform.position;

            if (distanceFromPlanet.magnitude < (nearestPlanet.transform.localScale.magnitude / 2f))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
            SimpleDebugDraw.Arrow(transform.position, rb.velocity, Color.blue);
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                SimpleDebugDraw.Arrow(transform.position, -gravityDir * jumpForce, Color.blue);

                rb.velocity = -gravityDir * jumpForce;
            }

            // Flying Up
            if (!isGrounded && Input.GetButton("Jump"))
            {
                SimpleDebugDraw.Arrow(transform.position, transform.up * flyAccel, Color.green);
                rb.AddForce(transform.up * flyAccel);
            }

            // Slowing Down (Might need tweaks or maybe not add)
            if (!isGrounded && Input.GetKey("s"))
            {
                SimpleDebugDraw.Arrow(transform.position, -transform.up * flyAccel, Color.green);
                rb.AddForce(-transform.up * flyAccel);
            }

            // Near planet effects
            AlignPlayerToClosestGravity();
        }
    }
    
    private void AlignPlayerToClosestGravity()
    {
        PlanetGravity closestPlanet = gravityEffects.GetClosetPlanet();
        float distanceFromPlanet = (closestPlanet.transform.position - transform.position).magnitude;

        if(distanceFromPlanet < (closestPlanet.transform.localScale.magnitude))
        {
            Vector2 gravityDirection = closestPlanet.GetGravityDirection(transform.position, closestPlanet.transform.position);

            float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * planetAlignmentSpeed);
        }
        
    }
}
