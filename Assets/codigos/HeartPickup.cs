using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    
    public int healAmount = 1;

   
    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.CompareTag("Player"))
        {
          
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            
            if (player != null)
            {
               
                bool didHeal = player.Heal(healAmount);

               
                if (didHeal)
                {
                   
                    Destroy(gameObject);
                }
            }
        }
    }
}