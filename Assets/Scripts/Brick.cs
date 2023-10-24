using UnityEngine;

public class Brick : MonoBehaviour
{
    public int maxHits = 1;
    public int scoreValue = 10;
    public GameObject destroyEffect;
    private int currentHits;
    private Collider2D brickCollider;

    private ScreenShake screenShake;
    private void Start()
    {
        currentHits = 0;
        brickCollider = GetComponent<Collider2D>();
        screenShake = FindObjectOfType<ScreenShake>();
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
}
