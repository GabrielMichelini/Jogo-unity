using UnityEngine;
using TMPro; 

public class CoinHudController : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI textoMoedas;

    
    public void AtualizarContagem(int quantidade)
    {
        if (textoMoedas != null)
        {
            textoMoedas.text = "Moedas: " + quantidade;
        }
    }
}