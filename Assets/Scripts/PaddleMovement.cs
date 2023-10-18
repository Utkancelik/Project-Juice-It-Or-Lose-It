using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 10f;     // Adjust the speed as needed.
    public float mobileMoveSpeed = 5f; // Mobile-specific speed.
    public bool mobileInputEnabled = true; // Enable or disable mobile input.

    private float paddleWidth;
    private float screenBoundsX;

    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    private bool isDragging = false;

    private void Start()
    {
        paddleWidth = transform.localScale.x; // Get the paddle's width.
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    private void Update()
    {
        // Handle paddle movement using keyboard or touch input.
        if (mobileInputEnabled && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        touchEndPos = touch.position;
                        float delta = touchEndPos.x - touchStartPos.x;
                        Vector3 newPosition = transform.position + Vector3.right * (delta / Screen.width) * mobileMoveSpeed * Time.deltaTime;
                        newPosition.x = Mathf.Clamp(newPosition.x, -screenBoundsX + paddleWidth / 2, screenBoundsX - paddleWidth / 2);
                        transform.position = newPosition;
                        touchStartPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
        else
        {
            // Get the horizontal input (left or right arrow key)
            float horizontalInput = Input.GetAxis("Horizontal");

            // Calculate the new position of the paddle
            Vector3 newPosition = transform.position + Vector3.right * horizontalInput * moveSpeed * Time.deltaTime;

            // Limit the paddle's movement within the screen boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, -screenBoundsX + paddleWidth / 2 + .5f, screenBoundsX - paddleWidth / 2 - .5f);

            // Apply the new position to the paddle
            transform.position = newPosition;
        }
    }
}
