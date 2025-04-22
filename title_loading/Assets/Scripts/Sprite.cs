using UnityEngine;

public class Sprite : MonoBehaviour
{
    // Components of sprite
    public Rigidbody2D rigidBody;
    //Animator variables 
    public Animator animator;
    // Physics & raycasting variables
    public float jumpForce = 18f;
    public float speed = 17f;
    public float castDistance;
    public LayerMask groundLayer; 
    private LayerMask combinedMask; 
    // Respawn & clamp variables
    public float fallThreshold = -17f; 
    public Vector2 respawnPoint;
    public float minX, maxX;
    public float maxY;
    // Animation variables
    private bool isFacingRight = true;
    // Level variables 
    public bool hasTriggeredExit = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        combinedMask = groundLayer | (1 << LayerMask.NameToLayer("Mouse"));
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Respawn sprite if falling below screen
        if (transform.position.y < fallThreshold)
        {
            AudioManager.Instance.DyingSound();
            Respawn();
        }

        UpdateVelocity();
        MirrorSprite();
        HandleJump();
        ClampPosition();
        UpdateAnimations(); 
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
            AudioManager.Instance.JumpSound();
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
        }
    }

     // Raycasting method to check if the user is on Ground 
    public bool isGrounded() {
        // Calculate the origin at the bottom of the sprite
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, groundLayer);
        //For debugging
        //Debug.DrawRay(transform.position, Vector2.down * castDistance, Color.red);
    
        if (hit.collider != null)
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
    public void Respawn()
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
        float clampedY = Mathf.Clamp(transform.position.y, fallThreshold, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // Checks if user has reached the exit point of the level and loads the next one
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggeredExit && other.CompareTag("Exit"))
        {
            AudioManager.Instance.ClearSound();
            hasTriggeredExit = true;
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            LevelManager.Instance.LoadNextLevel();
        }
    }

    // Upon a collision with the enemy, respawn them
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.DyingSound();
            Respawn();
        }
    }

   private void UpdateAnimations()
{
    bool grounded = isGrounded();
    bool upPressed = Input.GetKey(KeyCode.UpArrow);
    bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
    bool rightPressed = Input.GetKey(KeyCode.RightArrow);

    // Jumping animation
    if (upPressed && grounded)
    {
        animator.SetBool("isJumping", true);
        animator.SetFloat("Speed", 0);
    }
    // Walking animation
    else if ((leftPressed || rightPressed) && grounded)
    {
        animator.SetBool("isJumping", false);
        animator.SetFloat("Speed", 1); // Any non-zero value works
    }
    // Idle animation
    else if (grounded)
    {
        animator.SetBool("isJumping", false);
        animator.SetFloat("Speed", 0);
    }
    // In air without pressing jump (e.g., falling)
    else
    {
        animator.SetBool("isJumping", true);
        animator.SetFloat("Speed", 0);
    }
}
}
