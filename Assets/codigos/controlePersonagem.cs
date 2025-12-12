using UnityEngine;
using System.Collections;

public class controlePersonagem : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float moveSpeed = 5f;
    private float moveX;
    
    [Header("Sons")]
    public AudioClip jumpSound;
    public AudioClip attackSound;
    public AudioClip runSound;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        jumpCounter = extraJumps; 
    }

    void Update()
    {
        if (isDashing) return;

        moveX = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // --- SOM DE CORRER ---
        if (isGrounded && moveX != 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = runSound;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip == runSound && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        // ---------------------

        if (isGrounded)
        {
            jumpCounter = extraJumps;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (jumpCounter > 0)
            {
                Jump();
                jumpCounter--; 
            }
        }

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
        animator.SetFloat("VerticalSpeed", rb2d.velocity.y); 
        animator.SetBool("IsRunning", moveX != 0);       
        animator.SetBool("IsJumping", !isGrounded);

        if (moveX > 0 && !facingRight) Flip();
        else if (moveX < 0 && facingRight) Flip();

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
            rb2d.velocity = new Vector2(moveX * moveSpeed, rb2d.velocity.y); 
        }
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0); 
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce); 
        
        // Toca o som de pulo
        if(jumpSound != null) audioSource.PlayOneShot(jumpSound);
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
        // Toca o som de ataque
        if(attackSound != null) audioSource.PlayOneShot(attackSound);
    }
   
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale; 
        
        rb2d.gravityScale = 0f; 
        rb2d.velocity = new Vector2(transform.localScale.x * dashingPower, 0f); 
        
        if (tr != null) tr.emitting = true; 

        yield return new WaitForSeconds(dashingTime);

        if (tr != null) tr.emitting = false;
        
        rb2d.gravityScale = originalGravity; 
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}