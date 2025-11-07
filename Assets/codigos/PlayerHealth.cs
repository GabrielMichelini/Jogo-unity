using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3; 
    public int currentHealth; 

    [Header("Knockback")]
    public Rigidbody2D rb;
    public float knockbackForceX = 5f;
    public float knockbackForceY = 5f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount, Transform attacker)
    {
        currentHealth -= damageAmount;
        Debug.Log("Jogador tomou dano! Vida atual: " + currentHealth);

        int direction = (transform.position.x > attacker.position.x) ? 1 : -1;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(knockbackForceX * direction, knockbackForceY), ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jogador Morreu!");
        gameObject.SetActive(false);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}