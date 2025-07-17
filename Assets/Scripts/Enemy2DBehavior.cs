using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2DBehavior : MonoBehaviour
{
    [Header("Bewegung")]
    public float detectionRange = 6f;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform target;

    private Vector3 startPosition;

    GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (target == null)
            Debug.LogError("Kein Spieler gefunden – hat der Spieler den Tag 'Player'?");

        startPosition = transform.position;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.RegisterEnemy(gameObject);
        }
    }

    private void Update()
    {
        if (gameManager.GetIsInMenu()) return;
        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= detectionRange && distance > stopDistance)
        {
            MoveTowardsTarget();
        }
        else
        {
            StopMoving();
        }
    }

    void MoveTowardsTarget()
    {
        animator.Play("Slime Animation");

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1f, 1f);
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
        animator.Play("Slime Animation");
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}

