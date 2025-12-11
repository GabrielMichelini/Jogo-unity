using UnityEngine;

public class LightningSkill : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 1f; // Quanto tempo o raio fica na tela

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage, transform);
               
            }
        }
    }
}