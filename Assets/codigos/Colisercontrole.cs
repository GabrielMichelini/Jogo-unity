using UnityEngine;
using TMPro; // Importe o TextMeshPro se você o usa no CoinHudController

public class ControleColetaveis : MonoBehaviour
{
    [SerializeField] private int coinQtd = 0;
    
    // Variável para guardar o total de moedas
    private int totalMoedas; 
    
    private CoinHudController coinController;

    private void Awake()
    {
        // 1. Encontra o controlador da HUD (como você já fez)
        coinController = FindObjectOfType<CoinHudController>();

        // 2. CONTA todas as moedas na cena usando a tag
        totalMoedas = GameObject.FindGameObjectsWithTag("colector").Length;

        // 3. Atualiza a HUD pela primeira vez (ex: " = 0 / 10")
        if (coinController != null)
        {
            // CORRIGIDO: Envia os dois argumentos
            coinController.AtualizarContagem(coinQtd, totalMoedas);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "colector")
        {
            coinQtd = coinQtd + 1; 
            
            if (coinController != null)
            {
                // CORRIGIDO: Envia os dois argumentos
                coinController.AtualizarContagem(coinQtd, totalMoedas);
            }

            Destroy(other.gameObject);
        }
    }
}