using UnityEngine;
public enum BrickColor
{
    Green,
    Yellow,
    Red,
    Blue,
    // Add more colors here
}
public class Brick : MonoBehaviour
{
    public int maxHits = 3;
    public int scoreValue = 10;
    public GameObject destroyEffectPrefab;
    public BrickColor initialColor = BrickColor.Green; // Choose the initial color
    private SpriteRenderer spriteRenderer;

    private int currentHits;
    private Collider2D brickCollider;

    private Color[] hitColors;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHits = 0;
        brickCollider = GetComponent<Collider2D>();

        // Initialize hitColors array based on the initial color
        InitializeHitColors();

        SetInitialColor();
    }

    private void InitializeHitColors()
    {
        // You can set hitColors based on the initialColor enum
        switch (initialColor)
        {
            case BrickColor.Green:
                hitColors = new Color[] { Color.green, Color.yellow, Color.red, Color.blue };
                break;
            case BrickColor.Yellow:
                hitColors = new Color[] { Color.yellow, Color.red, Color.blue, Color.green };
                break;
            // Add cases for other colors
            default:
                hitColors = new Color[] { Color.green, Color.yellow, Color.red, Color.blue };
                break;
        }
    }

    private void SetInitialColor()
    {
        if (maxHits >= 1 && maxHits <= hitColors.Length)
        {
            spriteRenderer.color = hitColors[currentHits];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            currentHits++;
            if (currentHits >= maxHits)
            {
                DestroyBrick();
            }
            else
            {
                ChangeColorOnHit();
            }
        }
    }

    private void DestroyBrick()
    {
        brickCollider.enabled = false;
        GameManager.Instance.AddScore(scoreValue);

        if (destroyEffectPrefab != null)
        {
            GameObject destroyEffect = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
            Destroy(destroyEffect, 0.35f);
        }

        gameObject.SetActive(false);
    }

    private void ChangeColorOnHit()
    {
        if (currentHits < hitColors.Length)
        {
            spriteRenderer.color = hitColors[currentHits];
            Debug.Log("Changed color to " + hitColors[currentHits]);
        }
        else
        {
            Debug.LogWarning("No more colors defined for hits!");
        }
    }
}
