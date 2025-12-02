using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Slider healthBar;
    
    [Header("Efeito de Dano")]
    public SpriteRenderer spriteRenderer; 
    public Color damageColor = Color.red; 

    void Start()
    {
        currentHealth = maxHealth;
        
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            
          
            healthBar.gameObject.SetActive(false);
        }

        if (spriteRenderer == null) 
             spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    public void ShowHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        StartCoroutine(BlinkEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    IEnumerator BlinkEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damageColor; 
            yield return new WaitForSeconds(0.1f); 
            spriteRenderer.color = Color.white; 
        }
    }

    void Die()
    {
        Debug.Log("BOSS DERROTADO!");
        Destroy(gameObject); 
        
        if (healthBar != null) healthBar.gameObject.SetActive(false);
    }
}