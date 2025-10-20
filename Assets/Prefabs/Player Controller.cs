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
    private void Start()
    {
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
            rb.AddForce(moveInput * acceleration, ForceMode2D.Force);

            // Apperently this prefents drift buildup
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed; // Keep the momentum and keep going!
            }
        }
    }
}
