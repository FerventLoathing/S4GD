using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackV2 : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float projectileSpeed = 1f;
    MousePosition mousePosition;
    Vector2 attackDirection;
    [SerializeField] float projectileLifeDuration = 2f;
    [SerializeField] GameObject particlesHit;
    bool hasHit = false;

    void Start()
    {
        mousePosition = FindObjectOfType<MousePosition>();
        rb = GetComponent<Rigidbody2D>();

        attackDirection = new Vector2(
            mousePosition.GetMousePosition().x - transform.position.x,
            mousePosition.GetMousePosition().y - transform.position.y
        ).normalized;

        Destroy(gameObject, projectileLifeDuration);
        FlipSprite();
    }

    void FlipSprite()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(attackDirection.x), 1f);
        }
    }

    void Update()
    {
        rb.velocity = attackDirection * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Treffer gegen Gegner
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!hasHit)
            {
                Instantiate(particlesHit, collision.transform.position, transform.rotation);
                hasHit = true;

                // Schaden zufügen, falls EnemyHealth vorhanden
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }

            Destroy(gameObject, 0.05f);
        }

        // Treffer gegen Terrain
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject, 0.05f);
        }
    }
}

