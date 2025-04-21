using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private bool facingRight = true;
    public Transform groundCheck;
    public float groundCheckDist = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2((facingRight ? speed : -speed), rb.linearVelocity.y);
        if (groundCheck != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, groundLayer);
            // For debugging
            Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDist, Color.red);

            // Make sure mouse doesn't effect collisions with the enemy
            if (hit.collider == null)
            {
                Flip();
            }
        }
    }

    // Flip the enemy's moving direction
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

