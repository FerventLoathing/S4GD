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
        //attackDirection = (new Vector2(transform.position.x, transform.position.y) - mousePosition.GetMousePosition());
        //attackDirection = (mousePosition.GetMousePosition() - new Vector2(transform.position.x, transform.position.y));
        attackDirection = new Vector2(mousePosition.GetMousePosition().x - transform.position.x, mousePosition.GetMousePosition().y - transform.position.y).normalized;
        //attackDirection = mousePosition.GetMousePosition();
        Destroy(gameObject, projectileLifeDuration);
        FlipSprite();
    }
    void FlipSprite()
    {
        if (Mathf.Abs(attackDirection.x) > Mathf.Epsilon)
        {
            //this flips the player sprite on the vertical axis, according to the movement command
            transform.localScale = new Vector2(Mathf.Sign(attackDirection.x), 1f);
        }
    }
    void Update()
    {
        rb.velocity = (attackDirection * projectileSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 0.05f);
            if(!hasHit)
            {
                Instantiate(particlesHit, transform.position, transform.rotation);
                hasHit = true;
            }
            
        }

        if (collision.gameObject.tag == "Terrain")
        {
            Destroy(gameObject, 0.05f);
        }
    }
}
