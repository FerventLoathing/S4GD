using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    GameManager gameManager;
    bool wasInteractedWith = false;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !wasInteractedWith)
        {
            //Debug.Log("touched player");
            wasInteractedWith = true;
            gameManager.SetIsInMenu(true);
            gameManager.ToggleCanvasDialogue(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            wasInteractedWith = false;
        }
    }
}
