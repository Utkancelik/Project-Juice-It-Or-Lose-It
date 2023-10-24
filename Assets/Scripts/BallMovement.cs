using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public TrailRenderer trailRenderer; // Reference to the TrailRenderer component

    private Color originalStartColor;
    private Color originalEndColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialDirection.normalized * initialSpeed;
        gameManager = FindObjectOfType<GameManager>();
        screenShake = FindObjectOfType<ScreenShake>();

        // Store the original colors of the TrailRenderer
        originalStartColor = trailRenderer.startColor;
        originalEndColor = trailRenderer.endColor;
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
        if (collision.gameObject.CompareTag("Brick"))
        {
            // Trigger the screen shake
            screenShake.Shake(0.05f);

            // Change the color of the TrailRenderer
            trailRenderer.startColor = Color.white;
            trailRenderer.endColor = Color.white;

            // Start a coroutine to reset the colors with fading
            StartCoroutine(ResetTrailColorsWithFading());
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
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Trigger the screen shake and handle collision with the wall
            screenShake.Shake(0.1f);
            rb.velocity = rb.velocity.normalized * initialSpeed;
        }
    }

    private IEnumerator ResetTrailColorsWithFading()
    {
        float elapsedTime = 0f;
        float fadeDuration = 2f; // Adjust the fade duration as needed

        Color currentStartColor = trailRenderer.startColor;
        Color currentEndColor = trailRenderer.endColor;

        while (elapsedTime < fadeDuration)
        {
            trailRenderer.startColor = Color.Lerp(currentStartColor, originalStartColor, elapsedTime / fadeDuration);
            trailRenderer.endColor = Color.Lerp(currentEndColor, originalEndColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the colors are set to the original values
        trailRenderer.startColor = originalStartColor;
        trailRenderer.endColor = originalEndColor;
    }
}





