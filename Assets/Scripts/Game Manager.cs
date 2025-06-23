using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit(); // Beendet das Spiel
        /*
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Beendet das Spiel im Unity-Editor
#endif*/
    }

}
