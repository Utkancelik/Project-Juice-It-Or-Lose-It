using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        MultiBall,
        FireBall,
        BigPaddle,
        // Add more power-up types here
    }

    public PowerUpType powerUpType;
    public GameObject BallPrefab;
    public float effectDuration = 5.0f;
    public GameObject visualEffect;
    private Color originalStartColor;
    private Color originalEndColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Paddle"))
        {
            ApplyPowerUpEffect(other.gameObject);
            AudioManager.Instance.PlayPowerUpSound(); // Play power-up sound
            Destroy(gameObject);
        }
    }

    private void ApplyPowerUpEffect(GameObject paddle)
    {
        ApplyVisualFeedback();
        switch (powerUpType)
        {
            case PowerUpType.MultiBall:
                MultiBallPowerUp(paddle);
                break;
            case PowerUpType.FireBall:
                FireBallPowerUp();
                break;
            case PowerUpType.BigPaddle:
                BigPaddlePowerUp(paddle);
                break;
                // Add cases for other power-up types
        }
        StartCoroutine(RevertPowerUpEffect(paddle));
    }

    private void ApplyVisualFeedback()
    {
        GameObject visFX = Instantiate(visualEffect, transform.position, Quaternion.identity);
        Destroy(visFX, 0.35f);
    }

    private void MultiBallPowerUp(GameObject paddle)
    {
        Vector3 spawnPosition = paddle.transform.position + new Vector3(0, 0.5f, 0);
        GameObject extraBall1 = SpawnExtraBall(spawnPosition, true);
        GameObject extraBall2 = SpawnExtraBall(spawnPosition, true);
        extraBall1.GetComponent<BallMovement>().SetAsExtraBall();
        extraBall2.GetComponent<BallMovement>().SetAsExtraBall();
    }

    private GameObject SpawnExtraBall(Vector3 position, bool isRandomDirection)
    {
        GameObject newBall = Instantiate(BallPrefab, position, Quaternion.identity);
        Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
        if (isRandomDirection)
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(0.5f, 1f);
            rb.velocity = new Vector2(randomX, randomY).normalized * 10;
        }
        return newBall;
    }

    private void FireBallPowerUp()
    {
        BallMovement ballMovement = FindObjectOfType<BallMovement>();
        if (ballMovement != null)
        {
            ballMovement.ActivateFireBall();
        }
    }

    private void BigPaddlePowerUp(GameObject paddle)
    {
        paddle.transform.localScale *= 1.2f;
    }

    private IEnumerator RevertPowerUpEffect(GameObject paddle)
    {
        yield return new WaitForSeconds(effectDuration);
        RevertPowerUp(paddle);
    }

    private void RevertPowerUp(GameObject paddle)
    {
        switch (powerUpType)
        {
            case PowerUpType.BigPaddle:
                paddle.transform.localScale /= 1.2f;
                break;
            case PowerUpType.FireBall:
                RevertFireBallEffect();
                break;
                // Handle the reverting of other power-up effects here.
        }
    }

    private void RevertFireBallEffect()
    {
        BallMovement ballMovement = FindObjectOfType<BallMovement>();
        if (ballMovement != null)
        {
            ballMovement.RevertFireBall();
        }
    }
}
