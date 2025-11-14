using UnityEngine;
using TMPro; 
public class ControleColetaveis : MonoBehaviour
{
    [SerializeField] private int coinQtd = 0;
    
    
    private int totalMoedas; 
    
    private CoinHudController coinController;

    private void Awake()
    {
        
        coinController = FindObjectOfType<CoinHudController>();

        
        totalMoedas = GameObject.FindGameObjectsWithTag("colector").Length;

        
        if (coinController != null)
        {
           
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
               
                coinController.AtualizarContagem(coinQtd, totalMoedas);
            }

            Destroy(other.gameObject);
        }
    }
}