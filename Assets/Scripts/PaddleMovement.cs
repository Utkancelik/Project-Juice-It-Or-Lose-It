using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Adjust the speed as needed.
    public float mobileMoveSpeed = 5f; // Mobile-specific speed.
    public bool mobileInputEnabled = true; // Enable or disable mobile input.
    public GameObject leftButton; // Reference to the left movement button.
    public GameObject rightButton; // Reference to the right movement button.

    private float paddleWidth;
    private float screenBoundsX;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private void Start()
    {
        paddleWidth = transform.localScale.x; // Get the paddle's width.
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        // Add button click listeners
        leftButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(MoveLeft);
        rightButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(MoveRight);
    }

    private void Update()
    {
        // Handle paddle movement using keyboard or touch input.
        if (mobileInputEnabled)
        {
            // Check if the left button is pressed.
            if (isMovingLeft)
            {
                MovePaddle(-1);
            }
            // Check if the right button is pressed.
            if (isMovingRight)
            {
                MovePaddle(1);
            }
        }
        else
        {
            // Get the horizontal input (left or right arrow key)
            float horizontalInput = Input.GetAxis("Horizontal");
            MovePaddle(horizontalInput);
        }
    }

    private void MovePaddle(float direction)
    {
        Vector3 newPosition = transform.position + Vector3.right * direction * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBoundsX + paddleWidth / 2 + 0.5f, screenBoundsX - paddleWidth / 2 - 0.5f);
        transform.position = newPosition;
    }

    private void MoveLeft()
    {
        isMovingLeft = true;
        isMovingRight = false;
    }

    private void MoveRight()
    {
        isMovingRight = true;
        isMovingLeft = false;
    }

    public void StopMoving()
    {
        isMovingLeft = false;
        isMovingRight = false;
    }
}
