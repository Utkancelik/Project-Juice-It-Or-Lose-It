using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;

    public Text highScoreText;
    public Text roundText;
    public GameObject blackScreen;

    public void ShowGameOverScreen(int highScore, int currentRound)
    {
        // Display the GameOverCanvas and set the appropriate UI elements.
        gameCanvas.SetActive(false);  // Turn off the GameCanvas.
        gameOverCanvas.SetActive(true); // Turn on the GameOverCanvas.
        // Display the black screen and set the high score and round text
        highScoreText.text = "High Score: " + highScore;
        roundText.text = "Round " + currentRound;
        blackScreen.SetActive(true);
    }
}
