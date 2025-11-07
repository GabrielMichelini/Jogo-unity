using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Componentes")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    [Header("Patrulha")]
    public Transform[] patrolPoints; 
    public float moveSpeed = 2f;     

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
}