using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerData data;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement")]
    private float movementInputDirection;
    private float currentSpeed;
    private int facingDirection = 1;
    bool isFacingRight = true;
    bool isGrounded;
    bool canJump;
    bool canAttack = true;
    bool canMove = true;

    [Header("Check Surroudings")]
    public Transform groundCheckPos;
    public Transform checkEnemyPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = data.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateAnimation();
        CheckMovementDirection();
        CheckIfCanJump();


    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    void HandleInput()
    {

        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!canJump)
            {
                return;
            }
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AttackAnim();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpecialAttack();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, data.jumpForce);
    }

    void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;
            return;
        }

        canJump = false;
    }

    private void ApplyMovement()
    {
        if (!canMove)
        {
            return;
        }
        rb.velocity = new Vector2(currentSpeed * movementInputDirection * Time.deltaTime, rb.velocity.y);
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        facingDirection *= -1;

        transform.Rotate(0, 180, 0);
    }

    void UpdateAnimation()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, data.groundCheckRadius, data.groundLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckPos.position, data.groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkEnemyPos.position, data.enemyCheckRadius);
    }

    void AttackAnim()
    {
        if (!isGrounded)
        {
            return;
        }

        if (!canAttack)
        {
            return;
        }
        canAttack = false;

        currentSpeed = data.moveSpeedWhileAttacking;

        int attackIndex = UnityEngine.Random.Range(1, 3);
        anim.SetTrigger("attack" + attackIndex);
    }

    void SpecialAttack()
    {
        if (!isGrounded)
        {
            return;
        }

        if (!canAttack)
        {
            return;
        }
        canAttack = false;

        currentSpeed = data.moveSpeedWhileAttacking;

        anim.SetTrigger("specAttack");
    }

    public void ResetAttackAnim()
    {
        currentSpeed = data.moveSpeed;
        canAttack = true;
        anim.ResetTrigger("attack1");
        anim.ResetTrigger("attack2");
    }

    public void Dash()
    {
        StartCoroutine(EnumDash());
    }

    IEnumerator EnumDash()
    {
        yield return new WaitForEndOfFrame();

        if (!canMove)
        {
            yield break;
        }
        canMove = false; //disable move

        //dash
        rb.velocity = new Vector2(data.dashSpeed * facingDirection, rb.velocity.y);

        yield return new WaitForSeconds(0.5f); //wait
        canMove = true; //enable move
    }

    public void Attack()
    {
        float attackDirection = transform.position.x;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(checkEnemyPos.position, data.enemyCheckRadius, data.enemyLayer);

        if(enemies == null)
        {
            return;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyBase>().TakeDirection(attackDirection);
            enemies[i].GetComponent<EnemyBase>().TakeDamage(data.damageAmount);
            
        }
    }
}
