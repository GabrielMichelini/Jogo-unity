using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuração da Vitória")]
    public Transform victoryArea; 

    private int totalCoinsInLevel;
    private int coinsCollected = 0;
    private GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;

        
        totalCoinsInLevel = GameObject.FindGameObjectsWithTag("colector").Length;

        Debug.Log("Este nível tem " + totalCoinsInLevel + " moedas.");
    }

    public void OnCoinCollected()
    {
        coinsCollected++;
        Debug.Log("Moedas coletadas: " + coinsCollected);

        if (coinsCollected >= totalCoinsInLevel)
        {
            TeleportPlayerToVictory();
        }
    }

    void TeleportPlayerToVictory()
    {
        Debug.Log("VITÓRIA! Todas as moedas foram coletadas. Teleportando...");

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }

        player.transform.position = victoryArea.position;
    }
}