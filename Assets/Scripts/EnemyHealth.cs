using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.RegisterEnemy(gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Tod-Animation oder Partikel hier
        gameObject.SetActive(false);
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true); // falls separat aufgerufen wird
        transform.position = startPosition;

        // Optional: Animation zurücksetzen
        if (animator != null)
        {
            animator.Play("Slime Animation");
        }

        // Optional: Bewegung stoppen
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}


