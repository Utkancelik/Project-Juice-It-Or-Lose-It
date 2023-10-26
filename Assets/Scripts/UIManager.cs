using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameWonCanvas;
    public Text highScoreText;
    public Text roundText;
    public GameObject blackScreen;

    private void Start()
    {
        gameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(false);
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
        gameCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        gameWonCanvas.SetActive(true);
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}