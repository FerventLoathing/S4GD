using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator myAnimator;

    [Header("movement")]
    [SerializeField] float moveSpeed;
    Vector2 moveInput;
    Vector2 lastMoveInput;

    [Header("attack")]
    [SerializeField] GameObject attack;
    [SerializeField] Transform attackOrigin;
    [SerializeField] float attackLockoutDuration = 0.1f;
    float attackLockoutTimer;
    [SerializeField] float attackRootDuration = 0.2f;
    float attackRootTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Walk();
        FlipSprite();
        if(attackLockoutTimer >= attackLockoutDuration)
        {
            myAnimator.SetBool("isAttacking", false);
        }
    }

    void FixedUpdate()
    {
        attackLockoutTimer += Time.deltaTime;
        attackRootTimer += Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (attackLockoutDuration > attackLockoutTimer)
        {
            return;
        }
        Instantiate(attack, attackOrigin.position, transform.rotation);

        myAnimator.SetBool("isAttacking", true);
        attackLockoutTimer = 0f;
        attackRootTimer = 0f;
    }

    void Walk()
    {
        //Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        Vector2 playerVelocity = moveInput * moveSpeed;
        if (attackRootDuration > attackRootTimer)
        {
            playerVelocity = new Vector2(0, 0);
        }

        rb.velocity = playerVelocity;



        if ((Mathf.Abs(moveInput.x) > Mathf.Epsilon | Mathf.Abs(moveInput.y) > Mathf.Epsilon) && attackRootDuration <= attackRootTimer)
        {
            myAnimator.SetBool("isMoving", true);
            lastMoveInput = moveInput;
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
    }

    void FlipSprite()
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon)
        {
            //this flips the player sprite on the vertical axis, according to the movement command
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }

    public Vector2 getLastMoveInput()
    {
        return lastMoveInput;
    }
}
