using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float projectileSpeed = 1f;
    PlayerController player;
    Vector2 attackDirection;
    [SerializeField] float projectileLifeDuration = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        attackDirection = player.getLastMoveInput();
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
}
