using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Enemy"))
        {
           
            EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
           
            if (enemy != null)
            {
                enemy.Die();
            }
        }
    }
}