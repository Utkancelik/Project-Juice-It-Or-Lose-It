using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 5f; // Adjust the initial speed as needed
    public Vector2 initialDirection = Vector2.up; // Adjust the initial direction as needed

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the initial velocity of the ball
        rb.velocity = initialDirection.normalized * initialSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Adjust ball's velocity based on where it hits the paddle.
            float hitOffset = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 newDirection = new Vector2(hitOffset, 1).normalized;

            // Apply the new direction to the ball with the same speed.
            rb.velocity = newDirection * initialSpeed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            Debug.Log("Collision normal: " + collision.contacts[0].normal);

            rb.velocity = rb.velocity.normalized * initialSpeed;
        }
        else if (collision.gameObject.CompareTag("Brick"))
        {
            // Handle brick collision here, e.g., destroy the brick and reflect the ball.
            Destroy(collision.gameObject);

            // Normalize the direction after hitting the brick to maintain a consistent speed.
            rb.velocity = rb.velocity.normalized * initialSpeed;
        }
    }
}
