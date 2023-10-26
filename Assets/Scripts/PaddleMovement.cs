using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PaddleMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float mobileMoveSpeed = 5f;
    public bool mobileInputEnabled = true;
    public GameObject leftButton;
    public GameObject rightButton;
    public Vector2 maxScale = new Vector2(1.2f, 1.2f);
    public float scaleUpDuration = 0.1f;
    public float scaleDownDuration = 0.3f;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.1f;
    public GameObject wallLeft;
    public GameObject wallRight;

    private Vector2 originalScale;
    private Vector2 currentScale;
    private float paddleWidth;
    private float screenBoundsX;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isShaking = false;
    private Vector3 originalPosition;

    private void Start()
    {
        paddleWidth = transform.localScale.x;
        screenBoundsX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        originalScale = new Vector2(transform.localScale.x, transform.localScale.y);
        currentScale = originalScale;
        originalPosition = transform.position;
        leftButton.GetComponent<Button>().onClick.AddListener(MoveLeft);
        rightButton.GetComponent<Button>().onClick.AddListener(MoveRight);
    }

    private void Update()
    {
        if (mobileInputEnabled)
        {
            if (isMovingLeft)
            {
                MovePaddle(-1);
            }
            if (isMovingRight)
            {
                MovePaddle(1);
            }
        }
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            MovePaddle(horizontalInput);
        }
    }

    private void MovePaddle(float direction)
    {
        float leftWallPosition = wallLeft.transform.position.x + wallLeft.GetComponent<Renderer>().bounds.size.x / 2;
        float rightWallPosition = wallRight.transform.position.x - wallRight.GetComponent<Renderer>().bounds.size.x / 2;
        Vector3 newPosition = transform.position + Vector3.right * direction * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, leftWallPosition + paddleWidth / 2, rightWallPosition - paddleWidth / 2);
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
            float scalex = Mathf.Lerp(initialScale.x, maxScale.x, t);
            float scaley = Mathf.Lerp(initialScale.y, maxScale.y, t);
            currentScale = new Vector2(scalex, scaley);
            transform.localScale = new Vector3(scalex, scaley, transform.localScale.z);
            float yOffset = Mathf.Sin((Time.time - startTime) * Mathf.PI * 20.0f) * shakeMagnitude;
            Vector3 newPosition = originalPos + new Vector3(0, yOffset, 0);
            transform.position = newPosition;
            yield return null;
        }
        transform.position = originalPos;
        currentScale = initialScale;
        transform.localScale = new Vector3(initialScale.x, initialScale.y, transform.localScale.z);
    }
}