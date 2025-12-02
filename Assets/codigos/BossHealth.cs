using UnityEngine;
using UnityEngine.UI; 
public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public Slider healthBar; 
    void Start()
    {
        currentHealth = maxHealth;
        
      
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
       
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

       
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("BOSS DERROTADO!");
       
        Destroy(gameObject); 
    }
}