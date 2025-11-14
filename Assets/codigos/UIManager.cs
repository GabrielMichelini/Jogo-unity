using UnityEngine;
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    
    public GameObject gameOverPanel;

 
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    
    public void RestartGame()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}