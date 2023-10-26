using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public int initialLives = 3;
    public int pointsPerBrick = 10;
    public UIManager uiManager;

    public static GameManager Instance { get; private set; }

    private int score = 0;
    private int lives;
    public bool isGameOver = false;
    public bool isGameWon = false;

    private Brick[] bricks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        lives = initialLives;
        LoadHighScore();
        UpdateScoreText();
        UpdateLivesText();
        bricks = FindObjectsOfType<Brick>();
    }

    public void SaveHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void LoadHighScore()
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        // Use savedHighScore as needed, such as displaying it in your UI.
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        SaveHighScore();
        CheckGameWon();
    }

    public void RemoveLife()
    {
        lives--;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Reset the game, e.g., respawn the ball and paddle.
            // Implement this reset logic as needed for your game.
        }

        UpdateLivesText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "1 UP " + lives;
        }
    }

    public void CheckGameWon()
    {
        int remainingBricks = CountActiveBricks();
        if (remainingBricks-1 == 0)
        {
            isGameWon = true;
            ShowGameWonScreen();
        }
    }

    private int CountActiveBricks()
    {
        int count = 0;
        foreach (var brick in bricks)
        {
            if (brick.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }

    private void ShowGameWonScreen()
    {
        if (uiManager != null)
        {
            uiManager.ShowGameWonScreen();
        }
    }

    public void RestartGame()
    {
        isGameOver = false;
        isGameWon = false;
        Time.timeScale = 1;
        lives = initialLives;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGameAfterWin()
    {
        RestartGame();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        if (uiManager != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            int currentRound = 1;
            uiManager.ShowGameOverScreen(highScore, currentRound);
        }
    }
}
