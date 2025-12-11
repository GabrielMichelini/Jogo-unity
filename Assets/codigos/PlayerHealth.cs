using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3; 
    public int currentHealth; 

    [Header("Knockback")]
    public Rigidbody2D rb;
    public float knockbackForceX = 5f;
    public float knockbackForceY = 5f;

    [Header("Efeito de Dano")]
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;

    [Header("UI da Vida")]
    public Image[] hearts;
    public Sprite fullHeart;

    [Header("Managers")]
    public UIManager uiManager;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void TakeDamage(int damageAmount, Transform attacker)
    {
        currentHealth -= damageAmount;
        Debug.Log("Jogador tomou dano! Vida atual: " + currentHealth);

        UpdateHealthUI();
        StartCoroutine(FlashDamage());

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

    IEnumerator FlashDamage()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = Color.white;
        }
    }

    public void Die()
    {
        Debug.Log("Jogador Morreu!");
        gameObject.SetActive(false);
        uiManager.ShowGameOver();
    }

    public bool Heal(int healAmount)
    {
        if (currentHealth >= maxHealth)
        {
            return false;
        }

        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthUI();
        return true;
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