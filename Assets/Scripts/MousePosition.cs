using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    GameManager gameManager;
    Vector3 mouseWorldPosition;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
        //Debug.Log(mouseWorldPosition);

        if(gameManager.GetIsInMenu())
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    public Vector2 GetMousePosition()
    {
        Debug.Log(mouseWorldPosition);
        return new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
    }
}
