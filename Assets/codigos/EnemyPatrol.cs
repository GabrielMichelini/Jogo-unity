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
    public float bounceForce = 7f;

    private int currentPointIndex = 0; 
    private bool isFacingRight = true; 

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
            ContactPoint2D contact = collision.contacts[0];

            if (contact.normal.y > 0.7f)
            {
                Die();

                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0); 
                    playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }

    public void Die()
    {
        Debug.Log("Inimigo foi derrotado!");
        Destroy(gameObject);
    }
}