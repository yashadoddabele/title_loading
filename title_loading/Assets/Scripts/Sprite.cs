using UnityEngine;

public class Sprite : MonoBehaviour
{
    // Components of sprite
    public Rigidbody2D rigidBody;
    // Physics & raycasting variables
    public float jumpForce = 18f;
    public float speed = 17f;
    public float castDistance;
    public LayerMask groundLayer;  
    // Respawn & clamp variables
    public float fallThreshold = -17f; 
    public Vector2 respawnPoint;
    public float minX, maxX;
    // Animation variables
    private bool isFacingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn sprite if falling below screen
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }

        UpdateVelocity();
        MirrorSprite();
        HandleJump();
        ClampPosition();
    }

    // Updates horizontal and vertical velocity of sprite based on arrow keys
    private void UpdateVelocity() {
        float moveInput = Input.GetAxis("Horizontal");
        float nextXPosition = transform.position.x + moveInput * speed * Time.deltaTime;

        // Update the horizontal and vertical velocity of the sprite from key input
        if (nextXPosition >= minX && nextXPosition <= maxX)
        {
            rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);
        }
        else
        {
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
        }
    }

    // Flip's the sprite's image based on which direction it's facing
    private void MirrorSprite() {
        if (rigidBody.linearVelocity.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (rigidBody.linearVelocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Facilitates a jump mechanic for the sprite
    private void HandleJump() {
        // Jump if space is pressed and not already in air
        bool grounded = isGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        }
    }

     // Raycasting method to check if the user is on Ground 
    public bool isGrounded() {
        // Calculate the origin at the bottom of the sprite
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, groundLayer);
        //For debugging
        //Debug.DrawRay(transform.position, Vector2.down * castDistance, Color.red);
    
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            if (Vector2.Dot(hit.normal, Vector2.up) >= 0.9f)
            {
                return true;
            }
        }
        return false;
    }

    //Helper method to actually flip the sprite renderer
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Respawns sprite at designated respawn area for level
    private void Respawn()
    {
        Vector2 adjustedRespawn = respawnPoint;
        adjustedRespawn.y += 0.5f; // For better physics

        Debug.Log($"Respawning to: {adjustedRespawn.x}, {adjustedRespawn.y}");

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;

        transform.position = adjustedRespawn;
    }

    // Clamps sprite to bounds of the current level
    private void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
