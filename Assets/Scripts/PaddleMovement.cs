using System.Collections;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Adjust the speed as needed.
    public float mobileMoveSpeed = 5f; // Mobile-specific speed.
    public bool mobileInputEnabled = true; // Enable or disable mobile input.
    public GameObject leftButton; // Reference to the left movement button.
    public GameObject rightButton; // Reference to the right movement button.

    public Vector2 maxScale = new Vector2(1.2f, 1.2f); // Maximum scale when hit (x and y).
    public float scaleUpDuration = 0.1f; // Speed for scaling up.
    public float scaleDownDuration = 0.3f; // Speed for scaling down.

    public float shakeDuration = 0.2f; // Duration of the shake.
    public float shakeMagnitude = 0.1f; // Magnitude of the shake.

    private Vector2 originalScale; // Store the original scale.
    private Vector2 currentScale; // Store the current scale.

    private float paddleWidth;
    private float screenBoundsX;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isShaking = false;

    private Vector3 originalPosition; // Store the original position for shaking.

    public GameObject wallLeft; // Reference to the left movement button.
    public GameObject wallRight; // Reference to the right movement button.
    private void Start()
    {
        paddleWidth = transform.localScale.x; // Get the paddle's width.
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        // Store the original scale.
        originalScale = new Vector2(transform.localScale.x, transform.localScale.y);
        currentScale = originalScale;

        // Store the original position for shaking.
        originalPosition = transform.position;

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
        // Calculate the position of the walls based on the wall GameObjects.
        float leftWallPosition = wallLeft.transform.position.x + wallLeft.GetComponent<Renderer>().bounds.size.x / 2;
        float rightWallPosition = wallRight.transform.position.x - wallRight.GetComponent<Renderer>().bounds.size.x / 2;

        // Calculate the new position for the paddle.
        Vector3 newPosition = transform.position + Vector3.right * direction * moveSpeed * Time.deltaTime;

        // Clamp the paddle position to stay within the wall boundaries.
        newPosition.x = Mathf.Clamp(newPosition.x, leftWallPosition + paddleWidth / 2, rightWallPosition - paddleWidth / 2);

        // Apply the new position to the paddle.
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

    public void ScalePaddleOnHit()
    {
        StartCoroutine(ScaleAndShakePaddle());
    }

    private IEnumerator ScaleAndShakePaddle()
    {
        Vector2 initialScale = currentScale;
        Vector3 originalPos = transform.position;
        float startTime = Time.time;

        while (Time.time - startTime < shakeDuration)
        {
            float t = (Time.time - startTime) / shakeDuration;

            // Calculate the scale using Lerp to smoothly transition from initialScale to maxScale.
            float scalex = Mathf.Lerp(initialScale.x, maxScale.x, t);
            float scaley = Mathf.Lerp(initialScale.y, maxScale.y, t);
            currentScale = new Vector2(scalex, scaley);
            transform.localScale = new Vector3(scalex, scaley, transform.localScale.z);

            // Calculate the vertical shake position using Sin function.
            float yOffset = Mathf.Sin((Time.time - startTime) * Mathf.PI * 20.0f) * shakeMagnitude;
            Vector3 newPosition = originalPos + new Vector3(0, yOffset, 0);
            transform.position = newPosition;

            yield return null;
        }

        // Reset the paddle to its original state when the shaking is done.
        transform.position = originalPos;
        currentScale = initialScale;
        transform.localScale = new Vector3(initialScale.x, initialScale.y, transform.localScale.z);
    }

}
