using UnityEngine;
using TMPro; 

public class CoinHudController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoMoedas;

    public void AtualizarContagem(int quantidadeAtual, int totalMoedas)
    {
        if (textoMoedas != null)
        {
            textoMoedas.text = " = " + quantidadeAtual + " / " + totalMoedas;
        }
    }
}