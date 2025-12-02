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

    private int currentPointIndex = 0; 
    private bool isFacingRight = true; 

    void Start()
    {
        rb.freezeRotation = true;
    }

    void Update()
    {
        MoveTowardsWaypoint();

        if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.5f)
        {
            currentPointIndex++;
            if (currentPointIndex >= patrolPoints.Length)
            {
                currentPointIndex = 0;
            }
        }
    }

    void MoveTowardsWaypoint()
    {
        if (patrolPoints.Length == 0) return;

        Vector2 targetPosition = patrolPoints[currentPointIndex].position;
        
        float direction = (targetPosition.x > transform.position.x) ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (direction > 0 && !isFacingRight) Flip();
        else if (direction < 0 && isFacingRight) Flip();
    }

    public void Jump(float jumpForce)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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
        Destroy(gameObject);
    }
}