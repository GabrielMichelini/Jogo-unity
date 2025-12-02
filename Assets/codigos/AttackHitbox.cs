using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public int damageAmount = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        BossHealth boss = other.GetComponent<BossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(damageAmount);
            return; 
        }

      EnemyPatrol enemy = other.GetComponent<EnemyPatrol>();
        if (enemy != null)
        {
            enemy.Die(); 
        }
    }
}