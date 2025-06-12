using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 mouseWorldPosition;
    void Update()
    {
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
        //Debug.Log(mouseWorldPosition);
    }

    public Vector2 GetMousePosition()
    {
        Debug.Log(mouseWorldPosition);
        return new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
    }
}
