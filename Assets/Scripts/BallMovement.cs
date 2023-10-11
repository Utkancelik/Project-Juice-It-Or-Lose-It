using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 5f; // Adjust the initial speed as needed
    public Vector2 initialDirection = Vector2.up; // Adjust the initial direction as needed

    private Rigidbody2D rb;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Set the initial velocity of the ball
        rb.velocity = initialDirection.normalized * initialSpeed;
    }
}
