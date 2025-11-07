using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3; 
    public int currentHealth; 

    [Header("Knockback")]
    public Rigidbody2D rb;
    public float knockbackForceX = 5f;
    public float knockbackForceY = 5f;

    [Header("UI da Vida")]
    public Image[] hearts;
    public Sprite fullHeart;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount, Transform attacker)
    {
        currentHealth -= damageAmount;
        Debug.Log("Jogador tomou dano! Vida atual: " + currentHealth);

        UpdateHealthUI();

        int direction;
        if (transform.position.x < attacker.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

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

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}