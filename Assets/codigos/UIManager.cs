using UnityEngine;
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
           
            Time.timeScale = 0f; 
        }
    }

    public void RestartGame()
    {
        
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}