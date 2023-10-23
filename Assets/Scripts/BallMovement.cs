using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public GameManager gameManager;
    public int pointsForBrickCollision = 10;
    public float initialSpeed = 5f; // Adjust the initial speed as needed
    public float someYThreshold;
    public Vector2 initialDirection = Vector2.up; // Adjust the initial direction as needed

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the initial velocity of the ball
        rb.velocity = initialDirection.normalized * initialSpeed;

        gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        if (transform.position.y < someYThreshold)
        {
            // The ball has fallen below the paddle and off the screen.
            // Call RemoveLife to handle life removal.
            gameManager.RemoveLife();

            if (gameManager.isGameOver)
            {
                // Game over logic here, such as pausing the game or displaying a game over screen.
            }
            else
            {
                // Reset the ball's position and other relevant game state.
                // You may also reset the paddle's position here.
            }
        }


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

            // Access the GameManager and add points to the score
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(pointsForBrickCollision);
            }

            // Normalize the direction after hitting the brick to maintain a consistent speed.
            rb.velocity = rb.velocity.normalized * initialSpeed;
        }
    }
}
