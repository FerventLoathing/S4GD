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

    [Header("Health")]
    [SerializeField] int maxHealth = 5;
    int currentHealth;

    [Header("Respawn")]
    [SerializeField] Transform spawnPoint;

    [Header("Knockback & Invincibility")]
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] float knockbackDuration = 0.5f;
    [SerializeField] float invincibilityDuration = 1.5f;
    [SerializeField] SpriteRenderer spriteRenderer;

    bool isKnockedBack = false;
    float knockbackTimer = 0f;
    bool isInvincible = false;
    float invincibilityTimer = 0f;

    GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (gameManager.GetIsInMenu())
            return;

        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                rb.velocity = Vector2.zero;
            }
        }

        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            BlinkSprite();
            if (invincibilityTimer >= invincibilityDuration)
            {
                EndInvincibility();
            }
        }

        Walk();
        FlipSprite();

        if (attackLockoutTimer >= attackLockoutDuration)
        {
            myAnimator.SetBool("isAttacking", false);
        }

        if (staminaCurrent < staminaMax)
        {
            if (!staminaRecoveryPaused)
            {
                staminaCurrent += staminaRecoveryRate * Time.deltaTime;
                staminaCurrent = Mathf.Clamp(staminaCurrent, 0, staminaMax);
            }
            else
            {
                staminaRecoveryDelayTimer += Time.deltaTime;
                if (staminaRecoveryDelayTimer >= staminaRecoveryDelay)
                {
                    staminaRecoveryPaused = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (gameManager.GetIsInMenu())
            return;

        attackLockoutTimer += Time.deltaTime;
        attackRootTimer += Time.deltaTime;
        rangedAttackTimer += Time.deltaTime;

        if (attackLockoutTimer >= attackLockoutDuration && isMeleeAttackQueued)
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
        if (gameManager.GetIsInMenu() || isKnockedBack)
            return;

        if (staminaCurrent < Mathf.Epsilon)
            return;

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
        if (gameManager.GetIsInMenu() || isKnockedBack)
            return;

        if (rangedAttackCooldown > rangedAttackTimer)
            return;

        Instantiate(attack2, attackOrigin.position, transform.rotation);
        rangedAttackTimer = 0f;
    }

    void Walk()
    {
        if (isKnockedBack) return;

        Vector2 playerVelocity = moveInput * moveSpeed;
        if (attackRootDuration > attackRootTimer)
        {
            playerVelocity = Vector2.zero;
        }

        rb.velocity = playerVelocity;

        if ((Mathf.Abs(moveInput.x) > Mathf.Epsilon || Mathf.Abs(moveInput.y) > Mathf.Epsilon) && attackRootDuration <= attackRootTimer)
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
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }

    private void PauseStaminaRecovery()
    {
        staminaRecoveryPaused = true;
        staminaRecoveryDelayTimer = 0f;
    }

    public Vector2 GetLastMoveInput() => lastMoveInput;
    public float GetRangedAttackCooldown() => rangedAttackCooldown;
    public float GetRangedAttackTimer() => rangedAttackTimer;
    public float GetStaminaCurrent() => staminaCurrent;
    public float GetStaminaMax() => staminaMax;
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        Debug.Log("Player took damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockback();
            StartInvincibility();
        }
    }

    void ApplyKnockback()
    {
        isKnockedBack = true;
        knockbackTimer = 0f;

        Vector2 enemyPos = FindClosestEnemy();
        Vector2 knockDirection = ((Vector2)transform.position - enemyPos).normalized;
        rb.velocity = knockDirection * knockbackForce;

        myAnimator.SetBool("isMoving", false);
    }

    Vector2 FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        Vector2 closest = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy.transform.position;
            }
        }

        return closest;
    }

    void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = 0f;
    }

    void EndInvincibility()
    {
        isInvincible = false;
        spriteRenderer.enabled = true;
    }

    void BlinkSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = Mathf.FloorToInt(invincibilityTimer * 10f) % 2 == 0;
        }
    }

    void Die()
    {
        Debug.Log("Player died.");

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
        else
        {
            Debug.LogWarning("SpawnPoint not assigned in PlayerController!");
        }

        currentHealth = maxHealth;
        staminaCurrent = staminaMax;

        gameManager.ResetAllEnemies();
        gameManager.SetIsInMenu(true);
        gameManager.ToggleCanvasMenu(true);

        moveInput = Vector2.zero;
        lastMoveInput = Vector2.zero;
        rb.velocity = Vector2.zero;
        myAnimator.SetBool("isMoving", false);
        isKnockedBack = false;
        EndInvincibility();
    }
}





