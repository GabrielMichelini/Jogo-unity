using UnityEngine;
using System.Collections;

public class controlePersonagem : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float moveSpeed = 5f;
    private float moveX;
    
    [Header("Sons (Arraste aqui)")]
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip runSound;
    private AudioSource audioSource; // O tocador de som

    [Header("Pulo")]
    public float jumpForce = 10f;
    public int extraJumps = 1; 
    private int jumpCounter;   
    
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
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
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Pega o Audio Source do boneco
        jumpCounter = extraJumps; 
    }

    void Update()
    {
        if (isDashing) return;

        moveX = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // --- PULO ---
        if (isGrounded) jumpCounter = extraJumps;

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) Jump();
            else if (jumpCounter > 0)
            {
                Jump();
                jumpCounter--; 
            }
        }
        // ------------

        // ATAQUE
        if (hasAttacked)
        {
            timer += Time.deltaTime; 
            if (timer > 1f) { hasAttacked = false; timer = 0; }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                hasAttacked = true;
                Attack();
            }
        }
        
        // ANIMAÇÕES
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", rb2d.linearVelocity.y); 
        animator.SetBool("IsRunning", moveX != 0);       
        animator.SetBool("IsJumping", !isGrounded);

        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) StartCoroutine(Dash());

        Move();
    }

    void Move()
    {
        if (!isDashing) rb2d.linearVelocity = new Vector2(moveX * moveSpeed, rb2d.linearVelocity.y); 
    }

    void Jump()
    {
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0); 
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpForce); 
        // Nota: Tirei o som daqui para você colocar na animação se quiser,
        // mas para pulo, o ideal é manter aqui:
        PlayJumpSound(); 
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
        // O som será chamado pela animação agora!
    }
   
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale; 
        rb2d.gravityScale = 0f; 
        rb2d.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f); 
        if (tr != null) tr.emitting = true; 
        yield return new WaitForSeconds(dashingTime);
        if (tr != null) tr.emitting = false;
        rb2d.gravityScale = originalGravity; 
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    // --- FUNÇÕES PARA O ANIMATOR CHAMAR ---

    public void PlayRunSound()
    {
        // Chama essa função quando o pé tocar no chão na animação
        if(runSound != null) audioSource.PlayOneShot(runSound);
    }

    public void PlayAttackSound()
    {
        // Chama essa função no momento do corte da espada
        if(attackSound != null) audioSource.PlayOneShot(attackSound);
    }

    public void PlayJumpSound()
    {
        // Pode ser chamado na animação de pulo (frame 1) ou pelo código
        if(jumpSound != null) audioSource.PlayOneShot(jumpSound);
    }
}