 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;
    Animator myAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Walk();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Walk()
    {
        //Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        Vector2 playerVelocity = moveInput * moveSpeed;
        rb.velocity = playerVelocity;

        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon | Mathf.Abs(moveInput.y) > Mathf.Epsilon)
        {
            myAnimator.SetBool("isMoving", true);
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
}
