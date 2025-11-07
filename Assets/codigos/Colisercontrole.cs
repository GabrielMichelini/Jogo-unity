using UnityEngine;

public class ControleColetaveis : MonoBehaviour
{
    [SerializeField] private int coinQtd = 0;
    
    private CoinHudController coinController;


    private void Awake()
    {
  
        coinController = FindObjectOfType<CoinHudController>();

        if (coinController != null)
        {
            coinController.AtualizarContagem(coinQtd);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
  
        if (other.gameObject.tag == "colector")
        {
        
            coinQtd = coinQtd + 1; 

         
            if (coinController != null)
            {
                coinController.AtualizarContagem(coinQtd);
            }

            Destroy(other.gameObject);
        }
    }
}