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
    public TrailRenderer trailRenderer;
    public GameObject FallDownEffect;
    public float timeThreshold = 2f;

    private Rigidbody2D rb;
    private ScreenShake screenShake;
    private Color originalStartColor;
    private Color originalEndColor;
    private bool isScaling = false;
    private Vector3 originalScale;
    private float scaleMultiplier = 1.8f;
    private float timeMovingInSameDirection = 0f;
    private bool isMovingInSameDirection = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialDirection.normalized * initialSpeed;
        rb.freezeRotation = true;
        gameManager = FindObjectOfType<GameManager>();
        screenShake = FindObjectOfType<ScreenShake>();
        trailRenderer = GetComponent<TrailRenderer>();
        originalStartColor = trailRenderer.startColor;
        originalEndColor = trailRenderer.endColor;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (transform.position.y < someYThreshold)
        {
            if (!gameManager.isGameOver)
            {
                gameManager.RemoveLife();
                if (gameManager.isGameOver)
                {
                    // Game over logic here
                }
                else
                {
                    ResetBallPosition();
                }
            }
        }
        else
        {
            CheckSameDirection();
        }
    }

    private void ResetBallPosition()
    {
        transform.position = startPosition;
        rb.velocity = initialDirection.normalized * initialSpeed;
        rb.freezeRotation = true;
    }

    private void CheckSameDirection()
    {
        Vector2 currentDirection = rb.velocity.normalized;
        if (Vector2.Dot(currentDirection, initialDirection) > 0.95f)
        {
            timeMovingInSameDirection += Time.deltaTime;
            if (timeMovingInSameDirection > timeThreshold)
            {
                float randomForce = Random.Range(-0.1f, 0.1f);
                rb.AddForce(new Vector2(randomForce, 0), ForceMode2D.Impulse);
                isMovingInSameDirection = true;
            }
        }
        else
        {
            isMovingInSameDirection = false;
            timeMovingInSameDirection = 0f;
        }
    }

    private IEnumerator ScaleBallOnHit()
    {
        isScaling = true;
        transform.localScale = originalScale * scaleMultiplier;
        yield return new WaitForSeconds(0.1f);
        isScaling = false;
        transform.localScale = originalScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        trailRenderer.startColor = Color.white;
        trailRenderer.endColor = Color.white;
        StartCoroutine(ScaleBallOnHit());
        if (collision.gameObject.CompareTag("Brick"))
        {
            screenShake.Shake(0.1f);
            rb.freezeRotation = true;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            GameObject fallDownEffect = Instantiate(FallDownEffect, transform.position, Quaternion.identity);
            Destroy(fallDownEffect, .35f);
            PaddleMovement paddleMovement = collision.gameObject.GetComponent<PaddleMovement>();
            if (paddleMovement != null)
                paddleMovement.ScalePaddleOnHit();
            screenShake.Shake(0.05f);
            float hitOffset = (transform.position.x - collision.transform.position.x) / collision.collider.bounds.size.x;
            Vector2 newDirection = new Vector2(hitOffset, 1).normalized;
            rb.velocity = newDirection * initialSpeed;
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            screenShake.Shake(0.2f);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
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
        float resetDuration = .25f;
        yield return new WaitForSeconds(resetDuration);
        trailRenderer.startColor = originalStartColor;
        trailRenderer.endColor = originalEndColor;
        rb.freezeRotation = true;
    }
}
