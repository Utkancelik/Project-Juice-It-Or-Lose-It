using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
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

    public TrailRenderer trailRenderer; // Reference to the TrailRenderer component

    private Color originalStartColor;
    private Color originalEndColor;
    private bool isMovingInSameDirection = false;
    private float timeMovingInSameDirection = 0f;
    private float timeThreshold = 2f; // Adjust this threshold as needed

    private bool isScaling = false;
    private Vector3 originalScale;
    private float scaleMultiplier = 1.8f; // Adjust the multiplier as needed

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialDirection.normalized * initialSpeed;
        rb.freezeRotation = true; // Freeze rotation at the start
        gameManager = FindObjectOfType<GameManager>();
        screenShake = FindObjectOfType<ScreenShake>();
        trailRenderer = GetComponent<TrailRenderer>();

        // Store the original colors of the TrailRenderer
        originalStartColor = trailRenderer.startColor;
        originalEndColor = trailRenderer.endColor;

        originalScale = transform.localScale;
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
        else
        {
            // Check if the ball is moving in the same direction for a while
            Vector2 currentDirection = rb.velocity.normalized;
            if (Vector2.Dot(currentDirection, initialDirection) > 0.95f)
            {
                timeMovingInSameDirection += Time.deltaTime;

                if (timeMovingInSameDirection > timeThreshold)
                {
                    // Apply a small random force to prevent sticking
                    float randomForce = Random.Range(-0.1f, 0.1f); // Adjust the range as needed
                    rb.AddForce(new Vector2(randomForce, 0), ForceMode2D.Impulse);
                    isMovingInSameDirection = true;
                    Debug.Log("FORCEEEEEEE");
                }
            }
            else
            {
                isMovingInSameDirection = false;
                timeMovingInSameDirection = 0f;
            }
        }
    }

    private void ResetBallPosition()
    {
        transform.position = startPosition;
        rb.velocity = initialDirection.normalized * initialSpeed;
        rb.freezeRotation = true; // Freeze rotation on reset
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Change the color of the TrailRenderer instantly
        trailRenderer.startColor = Color.white;
        trailRenderer.endColor = Color.white;
        StartCoroutine(ScaleBallOnHit());
        if (collision.gameObject.CompareTag("Brick"))
        {
            // Trigger the screen shake
            screenShake.Shake(0.05f);

            

            rb.freezeRotation = true; // Freeze rotation after collision

            // Rotate the ball to match its velocity direction
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Start a coroutine to reset the color, shape, and scale
            
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            PaddleMovement paddleMovement = collision.gameObject.GetComponent<PaddleMovement>();
            if (paddleMovement != null)
                paddleMovement.ScalePaddleOnHit();
            screenShake.Shake(0.025f);
            float hitOffset = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 newDirection = new Vector2(hitOffset, 1).normalized;
            rb.velocity = newDirection * initialSpeed;

            // Rotate the ball to match its new direction
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Trigger the screen shake and handle collision with the top wall
            screenShake.Shake(0.1f);

            // Reflect the ball's vertical velocity
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

            // Rotate the ball to match the reflection direction
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(ResetTrailColorAndShape());
    }

    private IEnumerator ResetTrailColorAndShape()
    {
        float resetDuration = .25f; // Adjust the reset duration as needed

        // Wait for the specified duration
        yield return new WaitForSeconds(resetDuration);

        // Reset the color
        trailRenderer.startColor = originalStartColor;
        trailRenderer.endColor = originalEndColor;

        rb.freezeRotation = true; // Freeze rotation after resetting
    }

    private IEnumerator ScaleBallOnHit()
    {
        // Scale the ball up
        isScaling = true;
        transform.localScale = originalScale * scaleMultiplier;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f); // Adjust the duration as needed

        // Reset the scale to its original size
        isScaling = false;
        transform.localScale = originalScale;
    }
}
