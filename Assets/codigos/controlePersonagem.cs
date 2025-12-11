using UnityEngine;
using System.Collections;

public class controlePersonagem : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float moveSpeed = 5f;
    private float moveX;
    
    [Header("Pulo")]
    public float jumpForce = 10f;
    public int extraJumps = 1; // 1 = Pulo Duplo, 2 = Triplo, etc.
    private int jumpCounter;   // Contador interno
    
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    private int pulandohash = Animator.StringToHash("IsJumping");
    private int movendohash = Animator.StringToHash("IsRunning");
    private Animator animator;

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 7f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private bool facingRight = true;
    
    private float timer = 0;
    private bool hasAttacked = false;
    
    void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        jumpCounter = extraJumps; // Inicializa o contador
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        moveX = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // --- LÓGICA DO PULO DUPLO ---
        
        // Se encostar no chão, reseta os pulos extras
        if (isGrounded)
        {
            jumpCounter = extraJumps;
        }

        // Verifica o input de pulo
        if (Input.GetButtonDown("Jump"))
        {
            // Se estiver no chão, pula normal
            if (isGrounded)
            {
                Jump();
            }
            // Se NÃO estiver no chão, mas ainda tiver pulos extras
            else if (jumpCounter > 0)
            {
                Jump();
                jumpCounter--; // Gasta um pulo
            }
        }
        // -----------------------------

        if (hasAttacked)
        {
            timer += Time.deltaTime; 
            if (timer > 1f) 
            {
                hasAttacked = false;
                timer = 0; 
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                hasAttacked = true;
                Attack();
            }
        }
        
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", rb2d.linearVelocity.y); 
        animator.SetBool("IsRunning", moveX != 0);       
        animator.SetBool("IsJumping", !isGrounded);

        
        if (moveX > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveX < 0 && facingRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Move();
    }

    void Move()
    {
        if (!isDashing)
        {
            rb2d.linearVelocity = new Vector2(moveX * moveSpeed, rb2d.linearVelocity.y); 
        }
    }

    void Jump()
    {
        // Zera a velocidade Y antes de pular para o pulo duplo ser consistente
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0); 
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce); 
    }

    void Flip()
    {
        facingRight = !facingRight; 
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; 
        transform.localScale = scaler;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
   
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale; 
        
        rb2d.gravityScale = 0f; 
        rb2d.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f); 
        
        if (tr != null) 
        {
            tr.emitting = true; 
        }

        yield return new WaitForSeconds(dashingTime);

        if (tr != null)
        {
            tr.emitting = false;
        }
        rb2d.gravityScale = originalGravity; 
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}