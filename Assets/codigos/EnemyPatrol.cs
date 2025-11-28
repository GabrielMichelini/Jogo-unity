using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Patrulha")]
    public Transform[] patrolPoints; 
    public float moveSpeed = 2f;     

    [Header("Combate")]
    public int damage = 1;

    [Header("Pulo Automatico")]
    public float jumpForce = 5f;
    public float jumpInterval = 3f;
    private float jumpTimer;

    private int currentPointIndex = 0; 
    private bool isFacingRight = true; 

    void Start()
    {
        jumpTimer = jumpInterval;
    }

    void Update()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        Vector2 targetPosition = new Vector2(targetPoint.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, 
                                                targetPosition, 
                                                moveSpeed * Time.deltaTime);

        if (transform.position.x < targetPoint.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x > targetPoint.position.x && isFacingRight)
        {
            Flip();
        }

        if (Mathf.Abs(transform.position.x - targetPoint.position.x) < 0.1f)
        {
            currentPointIndex++;

            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = 0;
            }
        }

        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0)
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 0.001f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpTimer = jumpInterval; 
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, this.transform);
            }
        }
    }

    public void Die()
    {
        Debug.Log("Inimigo foi derrotado!");
        Destroy(gameObject);
    }
}