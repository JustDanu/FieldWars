using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using Unity.Mathematics;
using System;

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

    public GravityBody[] bodies;
    //public GravityEffected gravityEffects;
    private BodyState nearestPlanet;

    public bool isGrounded;
    private Vector2 gravityDir;

    public CameraFollow cameraPrefab;
    [SerializeField]
    private bool serverJumpHeld;
    [SerializeField]
    private bool serverJumpReleased;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnStartServer()
    {
        bodies = FindObjectsOfType<GravityBody>();
    }

    public override void OnStartClient()
    {
        bodies = FindObjectsOfType<GravityBody>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        CameraFollow cam = Instantiate(cameraPrefab);
        cam.SetTarget(transform);
    }

    private void Update()
    {
        if(!isLocalPlayer) return;

        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        Vector3 input = new Vector2(xMove, yMove).normalized;

        bool jumpHeld = false, jumpReleased = false;

        if(Input.GetButton("Jump")) jumpHeld = true;
        
        if(Input.GetButtonUp("Jump")) jumpReleased = true;

        CMDMove(input, jumpHeld, jumpReleased);

        // Mouse input
        Vector3 mouseScreenPosition = Input.mousePosition;
        
    }
    private void FixedUpdate()
    {
        if (!isServer) return;

        nearestPlanet = GetClosetPlanet();

        // Gravity direction
        Vector2 gravityVector = nearestPlanet.position - new Vector2(transform.position.x, transform.position.y);
        gravityDir = gravityVector.normalized;
        SimpleDebugDraw.Arrow(transform.position, gravityDir * 2f, Color.white);

        // Jumping and checks for on a planet
        float distanceFromPlanet = gravityVector.magnitude;

        if (distanceFromPlanet < nearestPlanet.size)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        SimpleDebugDraw.Arrow(transform.position, rb.velocity, Color.blue);

        // Movement on planet
        if (isGrounded)
        {
            Vector2 tangent = gravityDir.Perpendicular2();
            SimpleDebugDraw.Arrow(transform.position, tangent * moveInput.x * moveSpeed, Color.green);
            rb.AddForce(tangent * moveInput.x * moveSpeed);
        }

        // Movement in space left and right (still janky but works, idk about adding forces in the left and right) 
        // IDEA: Maybe cursor will be what rotates and rotate code goes there
        if (!isGrounded && moveInput.x != 0)
        {
            Vector2 moveDirection = (moveInput.x * transform.right).normalized;
            SimpleDebugDraw.Arrow(transform.position, transform.right * moveInput.x * moveSpeed, Color.green);
            rb.AddForce(moveInput.x * transform.right * moveSpeed);

            float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationForce);
        }


        // Flying Up
        if (!isGrounded && serverJumpHeld)
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

        if (isGrounded && serverJumpReleased)
        {
            SimpleDebugDraw.Arrow(transform.position, -gravityDir * jumpForce, Color.blue);

            rb.velocity = -gravityDir * jumpForce;

            serverJumpReleased = false;
        }

        // Near planet effects
        AlignPlayerToClosestGravity();
        
    }

    [Command]
    private void CMDMove(Vector2 input, bool jumpHeld, bool jumpPressed)
    {
        //Debug.Log("MOve Command!");
        moveInput = input;
        serverJumpHeld = jumpHeld;
        if(jumpPressed && isGrounded) serverJumpReleased = true;
    }
    
    private void AlignPlayerToClosestGravity()
    {
        BodyState closestPlanet = GetClosetPlanet();
        Vector2 distanceVector = closestPlanet.position - new Vector2(transform.position.x, transform.position.y);
        float distanceFromPlanet = distanceVector.magnitude;
        

        if(distanceFromPlanet < closestPlanet.size)
        {
            
            Vector2 gravityDirection = distanceVector.normalized;

            float angle = Mathf.Atan2(gravityDirection.y, gravityDirection.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * planetAlignmentSpeed);
        }
        
    }

    public BodyState GetClosetPlanet()
    {
        BodyState closestPlanet = new BodyState();
        Vector2 difference = Vector2.zero;
        foreach (var body in bodies)
        {
            if(this.gameObject == body.gameObject || !body.affectsGravity) continue; // Skip player checking itself or checking for something that doesnt gravity

            if (closestPlanet.Equals(new BodyState()))
            {
                closestPlanet = body.GetState();
                difference = new Vector2(transform.position.x, transform.position.y) - closestPlanet.position;
            }
            else
            {
                difference = new Vector2(transform.position.x, transform.position.y) - body.GetState().position;
                Vector2 newDifference = new Vector2(transform.position.x, transform.position.y) - closestPlanet.position;

                if (difference.magnitude < newDifference.magnitude)
                {
                    closestPlanet = body.GetState();
                }
            }
        }
        return closestPlanet;
    }
}
