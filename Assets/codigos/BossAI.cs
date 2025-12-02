using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Configurações")]
    public Transform player;
    public Animator animator;
    public float speed = 2f;
    public float chaseRange = 5f;   
    public float attackRange = 1.5f; 
    
    [Header("Combate")]
    public int damage = 1;
    public float attackCooldown = 2f; 
    public float damageDelay = 0.5f;  

    private float lastAttackTime;
    private bool isFacingRight = true;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

       

        if (distance < attackRange)
        {
           
            animator.SetFloat("Speed", 0); 
            
            if (Time.time > lastAttackTime + attackCooldown)
            {
                StartAttack();
            }
        }
        else if (distance < chaseRange)
        {
           
            MoveTowardsPlayer();
        }
        else
        {
           
            animator.SetFloat("Speed", 0);
        }

        LookAtPlayer();
    }

    void MoveTowardsPlayer()
    {
        animator.SetFloat("Speed", 1); 
        
      
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void LookAtPlayer()
    {
        
        if (transform.position.x > player.position.x && isFacingRight) Flip();
        else if (transform.position.x < player.position.x && !isFacingRight) Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    void StartAttack()
    {
       
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;

       
        Invoke("DealDamage", damageDelay);
    }

    void DealDamage()
    {
        
        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= attackRange + 0.5f) 
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform);
            }
        }
    }
}