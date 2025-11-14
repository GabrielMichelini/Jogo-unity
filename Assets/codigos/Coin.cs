using UnityEngine;

public class Coin : MonoBehaviour
{
    private LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("ERRO: O script Coin.cs não encontrou o LevelManager na cena!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelManager != null)
            {
                levelManager.OnCoinCollected();
            }
            Destroy(gameObject);
        }
    }
}