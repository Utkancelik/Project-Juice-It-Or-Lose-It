using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public GameManager gameManager;
    public int pointsForBrickCollision = 10;
    public float initialSpeed = 5f;
    public float someYThreshold;
    public Vector2 initialDirection = Vector2.up;
    public Vector3 startPosition;

    private Rigidbody2D rb;

    private ScreenShake screenShake;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialDirection.normalized * initialSpeed;
        gameManager = FindObjectOfType<GameManager>();
        screenShake = FindObjectOfType<ScreenShake>();
    }

    private void Update()
    {
        if (transform.position.y < someYThreshold)
        {
            // The ball has fallen below the paddle and off the screen.
            // Check if the game is not already over.
            if (!gameManager.isGameOver)
            {
                gameManager.RemoveLife(); // Remove only 1 life.

                if (gameManager.isGameOver)
                {
                    // Game over logic here, such as pausing the game or displaying a game over screen.
                }
                else
                {
                    // Reset the ball's position and other relevant game state.
                    ResetBallPosition(); // Call the reset method.
                }
            }
        }
    }

    private void ResetBallPosition()
    {
        transform.position = startPosition;
        rb.velocity = initialDirection.normalized * initialSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Trigger the screen shake
        

        if (collision.gameObject.CompareTag("Paddle"))
        {
            PaddleMovement paddleMovement = collision.gameObject.GetComponent<PaddleMovement>();
            if (paddleMovement != null)
                paddleMovement.ScalePaddleOnHit();
            screenShake.Shake(0.025f);
            float hitOffset = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 newDirection = new Vector2(hitOffset, 1).normalized;
            rb.velocity = newDirection * initialSpeed;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            screenShake.Shake(0.1f);
            rb.velocity = rb.velocity.normalized * initialSpeed;
        }
    }
}



