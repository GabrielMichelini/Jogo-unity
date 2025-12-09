using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuração da Vitória")]
    public Transform victoryArea; 

    [Header("UI")]
    public CoinHudController coinHud; // Arraste sua HUD aqui

    private int totalCoinsInLevel;
    private int coinsCollected = 0;
    private GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        
        
        totalCoinsInLevel = GameObject.FindGameObjectsWithTag("colector").Length;
        Debug.Log("Este nível tem " + totalCoinsInLevel + " moedas.");

       
        if (coinHud != null)
        {
            coinHud.AtualizarContagem(0, totalCoinsInLevel);
        }
    }

    public void OnCoinCollected()
    {
        coinsCollected++;
        Debug.Log("Moedas coletadas: " + coinsCollected); 

       
        if (coinHud != null)
        {
            coinHud.AtualizarContagem(coinsCollected, totalCoinsInLevel);
        }

        
        if (coinsCollected >= totalCoinsInLevel)
        {
            TeleportPlayerToVictory();
        }
    }

    void TeleportPlayerToVictory()
    {
        Debug.Log("VITÓRIA! ");

        if (victoryArea != null)
        {
           
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

           
            player.transform.position = victoryArea.position;
        }
        else
        {
            Debug.LogError("ERRO: Você esqueceu de arrastar a Victory Area no LevelManager!");
        }
    }
}