using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText; // Reference to the UI text displaying the score
    public Text livesText; // Reference to the UI text displaying the lives
    public int initialLives = 3; // Set the initial number of lives to 1 for a classic Arkanoid experience.
    public int pointsPerBrick = 10; // Set the number of points awarded for each brick.

    private int score = 0;
    private int lives;
    public bool isGameOver = false;

    public UIManager uiManager; // Reference to the UIManager script for game over UI.

    private void Start()
    {
        lives = initialLives;
        LoadHighScore(); // Load the high score when the game starts.
        UpdateScoreText();
        UpdateLivesText();
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
        // Call SaveHighScore when the player achieves a new high score.
        SaveHighScore();
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
            livesText.text = "1 UP " + lives; // Display "1 UP" and the remaining lives.
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over is Running!");
        isGameOver = true;
        Time.timeScale = 0; // Pause the game.

        if (uiManager != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0); // Retrieve the high score.
            int currentRound = 1; // Calculate the current round.
            uiManager.ShowGameOverScreen(highScore, currentRound);
        }
    }

    public void RestartGame()
    {
        // Reset the game state.
        isGameOver = false;
        Time.timeScale = 1; // Unpause the game.

        // Reset other game-related parameters, such as lives, score, etc.
        lives = initialLives;
        score = 0;

        // Implement logic to respawn the ball and paddle.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
