using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed

    private float paddleWidth;
    private float screenBoundsX;

    private void Start()
    {
        paddleWidth = transform.localScale.x; // Get the paddle's width
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    private void Update()
    {
        // Get the horizontal input (left or right arrow key)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the new position of the paddle
        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * moveSpeed * Time.deltaTime;

        // Limit the paddle's movement within the screen boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBoundsX + paddleWidth / 2, screenBoundsX - paddleWidth / 2);

        // Apply the new position to the paddle
        transform.position = newPosition;
    }
}
