using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isInMenu = true;
    [SerializeField] GameObject canvasMenu;
    [SerializeField] GameObject canvasDialogue;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isInMenu = true;
            ToggleCanvasMenu(true);
            //QuitGame();
        }
    }

    public void ContinueGame()
    {
        isInMenu = false;
        ToggleCanvasMenu(false);
    }

    public void CloseDialogue()
    {
        isInMenu = false;
        ToggleCanvasDialogue(false);
    }

    public void QuitGame()
    {
        Application.Quit(); // Beendet das Spiel

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Beendet das Spiel im Unity-Editor
#endif
    }

    public bool GetIsInMenu()
    {
        return isInMenu;
    }

    public void SetIsInMenu(bool value)
    {
        isInMenu = value;
    }

    private void ToggleCanvasMenu(bool active)
    {
        canvasMenu.SetActive(active);
    }

    public void ToggleCanvasDialogue(bool active)
    {
        canvasDialogue.SetActive(active);
    }
}
