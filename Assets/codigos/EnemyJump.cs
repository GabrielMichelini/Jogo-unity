using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SendMessage("Jump", jumpForce, SendMessageOptions.DontRequireReceiver);
        }
    }
}