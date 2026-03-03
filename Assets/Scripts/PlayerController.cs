using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.IO;
using System.Threading;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;
    public bool isGrounded;
    public float airControlMultiplier = 0.8f;


    [Header("Görsel Ayarlar")]
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite ballSprite;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 10f;
    public float jumpForce = 14f;
    public float groundAcceleration = 100f;
    public float airAcceleration = 50f;

    [Header("Dash Ayarları")]
    public float dashForce = 30f;
    public float dashDuration = 0.15f;
    public bool isDashing = false;
    public bool dashAvaible = true;

    [Header("Collider Ayarları")]
    public CapsuleCollider2D playerCollider;
    public Vector2 normalSize = new Vector2(1f, 2f);
    public Vector2 ballSize = new Vector2(1f, 1f); 
    public Vector2 normalOffset = Vector2.zero;

    public Rigidbody2D rb;
    private float defaultGravity;


  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);



        if (isDashing) return;

        HandleMovement();
        HandleJump();
        HandleDashInput();
        UpdateVisuals();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float targetVelocityX = moveInput * moveSpeed;


        float acceleration = isGrounded ? groundAcceleration : airAcceleration;


        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetVelocityX, acceleration * Time.deltaTime);

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleDashInput()
    {

        if (isGrounded) dashAvaible = true;


        if (Input.GetMouseButtonDown(0))
        {
            if (dashAvaible == true)
            {
               StartCoroutine(PerformDash());
            }
        }
    }

 

    IEnumerator PerformDash()
    {
       
        isDashing = true;

        rb.gravityScale = 0; 
        dashAvaible = false;

        playerCollider.size = ballSize;
        
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        Vector2 dashDir = direction.normalized;
        rb.linearVelocity = dashDir * dashForce;


        yield return new WaitForSeconds(dashDuration);

       
        rb.gravityScale = defaultGravity;



        yield return new WaitUntil(() => isGrounded == true);

        playerCollider.size = normalSize;
        playerCollider.offset = normalOffset;

        isDashing = false;

    }

    public void DashCombo()
    {

        StopAllCoroutines();

       
        isDashing = false;
        rb.gravityScale = defaultGravity;
        dashAvaible = true;
        
        playerCollider.size = normalSize;
        playerCollider.offset = normalOffset;

        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
         
    }

   
    void UpdateVisuals()
    {
        if (isDashing)
        {
            spriteRenderer.sprite = ballSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }


       
        if (rb.linearVelocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.linearVelocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
     
    }


}