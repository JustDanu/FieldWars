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

    private GravityBody[] bodies;
    //public GravityEffected gravityEffects;
    private BodyState nearestPlanet;

    public bool isGrounded;
    private Vector2 gravityDir;
    private Vector3 mouseScreenPosition;

    private void Start()
    {
        bodies = FindObjectsOfType<GravityBody>();
        //gravityEffects = this.GetComponent<GravityEffected>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(isLocalPlayer)
        {
            float xMove = Input.GetAxis("Horizontal");
            float yMove = Input.GetAxis("Vertical");

            moveInput = new Vector2(xMove, yMove).normalized;

            // Player Ground Check
            nearestPlanet = GetClosetPlanet();

            // Gravity direction
            Vector2 gravityVector = nearestPlanet.position - transform.position;
            gravityDir = gravityVector.normalized;
            SimpleDebugDraw.Arrow(transform.position, gravityDir * 2f, Color.green);

            // Jumping and checks for on a planet
            float distanceFromPlanet = gravityVector.magnitude;

            if (distanceFromPlanet < (nearestPlanet.size))
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

            // Mouse input
            Vector3 mouseScreenPosition = Input.mousePosition;
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
        BodyState closestPlanet = GetClosetPlanet();
        Vector2 distanceVector = closestPlanet.position - transform.position;
        float distanceFromPlanet = distanceVector.magnitude;
        

        if(distanceFromPlanet < (closestPlanet.transform.localScale.magnitude))
        {
            Vector2 gravityDirection = distanceVector.normalized;

            float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * planetAlignmentSpeed);
        }
        
    }

    public BodyState GetClosetPlanet()
    {
        BodyState closestPlanet = null;
        Vector2 difference = Vector2.zero;
        foreach (BodyState body in bodies)
        {
            if (closestPlanet == null)
            {
                closestPlanet = body;
                difference = transform.position - body.position;
            }
            else
            {
                difference = transform.position - body.position;
                Vector2 newDifference = transform.position - closestPlanet.position;

                if (difference.magnitude < newDifference.magnitude)
                {
                    closestPlanet = planet;
                }
            }
        }
        return closestPlanet;
    }
}
