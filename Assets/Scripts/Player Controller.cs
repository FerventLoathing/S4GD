using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    float diagonalMoveLimiter = 0.7f;

    void Start()
    {
        
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        if (vertical != 0 && horizontal != 0)
        {
            horizontal = horizontal * diagonalMoveLimiter;
            vertical = vertical * diagonalMoveLimiter;
        }

        transform.Translate(horizontal, vertical, 0, Space.World);
    }
}
