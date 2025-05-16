using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DBehavior : MonoBehaviour
{
    [Header("Detection and Movement")]
    public float detectionRange = 10f;
    public float moveSpeed = 2f;

    [Header("Attack Settings")]
    public float stopDistance = 2f;
    public float windUpDuration = 1f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.4f;
    public float recoveryDuration = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;
    private bool isRecovering = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null || isAttacking || isRecovering) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (distance > stopDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StartCoroutine(AttackSequence());
            }
        }
        else
        {
            animator.Play("Idle");
        }
    }

    void MoveTowardsPlayer()
    {
        animator.Play("Move");

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

        FlipSprite(direction);
    }

    void FlipSprite(Vector2 direction)
    {
        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }

    System.Collections.IEnumerator AttackSequence()
    {
        isAttacking = true;

        animator.Play("Windup");
        yield return new WaitForSeconds(windUpDuration);

        animator.Play("Dash");
        float elapsed = 0f;
        Vector2 dashDirection = (player.position - transform.position).normalized;

        while (elapsed < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        animator.Play("Recovery");
        isRecovering = true;
        yield return new WaitForSeconds(recoveryDuration);

        isRecovering = false;
        isAttacking = false;
    }
}

