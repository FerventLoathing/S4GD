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
    [SerializeField] GameObject attack2;
    [SerializeField] Transform attackOrigin;
    [SerializeField] float attackLockoutDuration = 0.1f;
    float attackLockoutTimer;
    [SerializeField] float attackRootDuration = 0.2f;
    float attackRootTimer;
    [SerializeField] float rangedAttackCooldown = 5f;
    float rangedAttackTimer = 100;
    bool isMeleeAttackQueued = false;
    [SerializeField] float meleeStaminaCost = 20f;

    [Header("Stamina")]
    float staminaCurrent = 0;
    [SerializeField] float staminaMax = 100f;
    [SerializeField] float staminaRecoveryRate = 10f;
    [SerializeField] float staminaRecoveryDelay;
    float staminaRecoveryDelayTimer;
    bool staminaRecoveryPaused;

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
        
        if(staminaCurrent < staminaMax)
        {
            if (!staminaRecoveryPaused)
            {
                staminaCurrent += staminaRecoveryRate * Time.deltaTime;
                staminaCurrent = Mathf.Clamp(staminaCurrent += staminaRecoveryRate * Time.deltaTime, staminaCurrent, staminaMax);
            }

            else
            {
                staminaRecoveryDelayTimer += Time.deltaTime;
                if(staminaRecoveryDelayTimer >= staminaRecoveryDelay)
                {
                    staminaRecoveryPaused = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        attackLockoutTimer += Time.deltaTime;
        attackRootTimer += Time.deltaTime;
        rangedAttackTimer += Time.deltaTime;
        //Debug.Log(rangedAttackTimer);

        if(attackLockoutTimer >= attackLockoutDuration && isMeleeAttackQueued)
        {
            OnAttackMelee();
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnAttackMelee()
    {
        if(staminaCurrent < Mathf.Epsilon)
        {
            return;
        }

        isMeleeAttackQueued = false;
        if (attackLockoutDuration > attackLockoutTimer)
        {
            isMeleeAttackQueued = true;
            return;
        }

        staminaCurrent -= meleeStaminaCost;
        PauseStaminaRecovery();

        Instantiate(attack, attackOrigin.position, transform.rotation);

        myAnimator.SetBool("isAttacking", true);
        attackLockoutTimer = 0f;
        attackRootTimer = 0f;

    }

    void OnAttackRanged(InputValue value)
    {
        if (rangedAttackCooldown > rangedAttackTimer)
        {
            return;
        }
        Instantiate(attack2, attackOrigin.position, transform.rotation);
        rangedAttackTimer = 0f;
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

    private void PauseStaminaRecovery()
    {
        staminaRecoveryPaused = true;
        staminaRecoveryDelayTimer = 0f;
    }

    public Vector2 GetLastMoveInput()
    {
        return lastMoveInput;
    }
    public float GetRangedAttackCooldown()
    {
        return rangedAttackCooldown;
    }
    public float GetRangedAttackTimer()
    {
        return rangedAttackTimer;
    }

    public float GetStaminaCurrent()
    {
        return staminaCurrent;
    }

    public float GetStaminaMax()
    {
        return staminaMax;
    }
}
