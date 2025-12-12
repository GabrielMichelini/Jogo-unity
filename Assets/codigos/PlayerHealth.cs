using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3; 
    public int currentHealth; 

    [Header("Sons")]
    public AudioClip hurtSound;
    private AudioSource audioSource;

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
        
        // Pega o AudioSource (se não tiver no Player, ele tenta achar)
        audioSource = GetComponent<AudioSource>();

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount, Transform attacker)
    {
        currentHealth -= damageAmount;

        // Toca o som de dano
        if (hurtSound != null && audioSource != null) 
             audioSource.PlayOneShot(hurtSound);

        UpdateHealthUI();
        StartCoroutine(FlashDamage());

        if (CameraShake.instance != null) CameraShake.instance.Shake(0.3f, 0.2f); 

        int direction = (transform.position.x < attacker.position.x) ? -1 : 1;

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(knockbackForceX * direction, knockbackForceY), ForceMode2D.Impulse);

        if (currentHealth <= 0) Die();
    }

    IEnumerator FlashDamage()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        if(uiManager != null) uiManager.ShowGameOver();
    }

    public bool Heal(int healAmount)
    {
        if (currentHealth >= maxHealth) return false;

        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHealthUI();
        return true;
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