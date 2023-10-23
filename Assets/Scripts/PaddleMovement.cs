using UnityEngine;
using UnityEngine.UI;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Adjust the speed as needed.
    public float mobileMoveSpeed = 5f; // Mobile-specific speed.
    public bool mobileInputEnabled = true; // Enable or disable mobile input.

    private float paddleWidth;
    private float screenBoundsX;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    public Button leftButton; // Reference to the left movement button.
    public Button rightButton; // Reference to the right movement button.

    private void Start()
    {
        paddleWidth = transform.localScale.x; // Get the paddle's width.
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (mobileInputEnabled)
        {
            // Attach button click listeners
            leftButton.onClick.AddListener(StartMoveLeft);
            leftButton.onClick.AddListener(StopMoving);
            rightButton.onClick.AddListener(StartMoveRight);
            rightButton.onClick.AddListener(StopMoving);
        }
    }

    private void Update()
    {
        float direction = 0f;

        if (isMovingLeft)
        {
            direction = -1f;
        }
        else if (isMovingRight)
        {
            direction = 1f;
        }

        Vector3 newPosition = transform.position + Vector3.right * direction * (mobileInputEnabled ? mobileMoveSpeed : moveSpeed) * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBoundsX + paddleWidth / 2, screenBoundsX - paddleWidth / 2);
        transform.position = newPosition;
    }

    private void StartMoveLeft()
    {
        isMovingLeft = true;
    }

    private void StartMoveRight()
    {
        isMovingRight = true;
    }

    private void StopMoving()
    {
        isMovingLeft = false;
        isMovingRight = false;
    }
}
