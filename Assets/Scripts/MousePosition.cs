using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector3 mouseWorldPosition;
    void Update()
    {
        //Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }

    public Vector2 GetMousePosition()
    {
        return new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
    }
}
