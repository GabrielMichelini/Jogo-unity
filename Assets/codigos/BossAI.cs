using UnityEngine;
using System.Collections;

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

    [Header("Ataque Especial (Pulo)")]
    public GameObject lightningPrefab; 
    public float jumpForce = 15000f; 
    public int lightningCount = 5; 
    public float lightningDelay = 0.6f; // Tempo para o jogador desviar

    private float lastAttackTime;
    private bool isFacingRight = true;
    private bool isBusy = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (lightningPrefab != null)
            Debug.Log("BOSS CHECK: O Raio está configurado corretamente!");
        else
            Debug.LogError("BOSS CHECK: O campo Raio está VAZIO! Arraste o prefab agora.");
    }

    void Update()
    {
        if (player == null || isBusy) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            animator.SetFloat("Speed", 0);
            
            if (Time.time > lastAttackTime + attackCooldown)
            {
                int randomAttack = Random.Range(0, 2); 
                
                if (randomAttack == 0) StartAttack();
                else StartCoroutine(PerformJumpAttack());
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
        isBusy = true;
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        Invoke("DealDamage", damageDelay);
        Invoke("ResetBusy", 1f); 
    }

    void DealDamage()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange + 1f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(damage, transform);
        }
    }

    void ResetBusy()
    {
        isBusy = false;
    }

    IEnumerator PerformJumpAttack()
    {
        isBusy = true;
        lastAttackTime = Time.time;
        animator.SetFloat("Speed", 0);
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1.0f); 

        SpawnLightning();

        yield return new WaitForSeconds(1f);
        isBusy = false;
    }

    void SpawnLightning()
    {
        if (lightningPrefab == null)
        {
            Debug.LogError("ERRO: O Raio sumiu!");
            return;
        }

        Debug.Log("BOSS: TRAVANDO MIRA NO JOGADOR...");

        // 1. INICIA O RAIO MIRA (NO JOGADOR)
        // Passa a posição ATUAL do jogador para a corrotina
        StartCoroutine(SpawnDelayedLightning(player.position));

        // 2. RAIOS ALEATÓRIOS (CAEM NA HORA)
        for (int i = 0; i < lightningCount; i++)
        {
            float randomX = Random.Range(-5f, 5f);
            Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, 0f);
            Instantiate(lightningPrefab, spawnPos, Quaternion.identity);
        }
    }

    // --- NOVA FUNÇÃO: ESPERA ANTES DE CRIAR O RAIO ---
    IEnumerator SpawnDelayedLightning(Vector3 targetPosition)
    {
        // Espera o tempo configurado (ex: 0.6 segundos)
        // Durante esse tempo, o jogador deve sair da posição 'targetPosition'
        yield return new WaitForSeconds(lightningDelay);

        // Cria o raio onde o jogador ESTAVA (não onde ele está agora)
        Instantiate(lightningPrefab, targetPosition, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}