using UnityEngine;

public class Brick : MonoBehaviour
{
    public int maxHits = 2; // Set the maximum hits required
    public int scoreValue = 10;
    public GameObject destroyEffect;
    private int currentHits;
    private Collider2D brickCollider;

    private ScreenShake screenShake;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHits = 0;
        brickCollider = GetComponent<Collider2D>();
        screenShake = FindObjectOfType<ScreenShake>();
        spriteRenderer = GetComponent <SpriteRenderer>(); // Get the SpriteRenderer component

        // Set the initial color based on the maxHits value
        SetInitialColor();
    }
    private void SetInitialColor()
    {
        Color[] initialColors = new Color[] { Color.green, Color.yellow, Color.red };

        if (maxHits >= 1 && maxHits <= initialColors.Length)
        {
            spriteRenderer.color = initialColors[maxHits - 1];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            screenShake.Shake(0.05f);

            currentHits++;

            if (currentHits >= maxHits)
            {
                Debug.Log("Brick destroyed!");
                DestroyBrick();
            }
            else
            {
                // Play a hit effect or change the brick's appearance, for example.
                // You can implement this as needed.
                // Example: Change the brick's color based on hit count
                ChangeColorOnHit();
            }
        }
    }

    protected void DestroyBrick()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Disable the collider to prevent further collisions
        brickCollider.enabled = false;

        // Check for remaining bricks before adding score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        // Destroy the brick after checking for game won
        Invoke("DestroyAfterDelay", 0f);
    }

    private void DestroyAfterDelay()
    {
        Destroy(gameObject);
    }

    private void ChangeColorOnHit()
    {
        Color[] hitColors = new Color[] { Color.green, Color.yellow, Color.red };

        if (maxHits >= 1 && currentHits <= maxHits && currentHits <= hitColors.Length)
        {
            spriteRenderer.color = hitColors[currentHits - 1];
        }
    }

}
