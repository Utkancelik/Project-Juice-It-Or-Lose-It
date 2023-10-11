using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Handle ball-paddle collision here
            // You can adjust the ball's direction based on where it hits the paddle
            BounceOffPaddle(collision.contacts[0].point, collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            // Handle ball-brick collision here
            // You can destroy the brick and adjust the ball's direction
            Destroy(other.gameObject);
            BounceOffBrick();
        }
    }

    // Function to bounce off the paddle
    private void BounceOffPaddle(Vector2 contactPoint, Transform paddle)
    {
        float paddleWidth = paddle.localScale.x;
        float offset = (transform.position.x - contactPoint.x) / paddleWidth;
        Vector2 newDirection = new Vector2(offset, 1).normalized;
        GetComponent<Rigidbody2D>().velocity = newDirection * GetComponent<Rigidbody2D>().velocity.magnitude;
    }

    // Function to change the ball's direction when hitting a brick
    private void BounceOffBrick()
    {
        // Change the direction as needed when the ball hits a brick
        Vector2 newDirection = new Vector2(0, 1);
        GetComponent<Rigidbody2D>().velocity = newDirection * GetComponent<Rigidbody2D>().velocity.magnitude;
    }
}
