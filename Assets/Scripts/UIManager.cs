using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameWonCanvas; // Add a reference to the GameWonCanvas.
    public Text highScoreText;
    public Text roundText;
    public GameObject blackScreen;

    private void Start()
    {
        gameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(false); // Initially turn off the GameWonCanvas.
    }

    public void ShowGameOverScreen(int highScore, int currentRound)
    {
        gameCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        highScoreText.text = "High Score: " + highScore;
        roundText.text = "Round " + currentRound;
        blackScreen.SetActive(true);
    }

    public void ShowGameWonScreen()
    {
        // Display the GameWonCanvas and set the appropriate UI elements.
        gameCanvas.SetActive(false); // Turn off the GameCanvas.
        gameOverCanvas.SetActive(false); // Turn off the GameOverCanvas.
        gameWonCanvas.SetActive(true); // Turn on the GameWonCanvas.
    }


    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
