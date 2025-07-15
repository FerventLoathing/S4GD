using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isInMenu = true;
    [SerializeField] GameObject canvasMenu;
    [SerializeField] GameObject canvasDialogue;

    // Liste aller Gegner
    private List<GameObject> allEnemies = new List<GameObject>();

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
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
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

    public void ToggleCanvasMenu(bool active)
    {
        canvasMenu.SetActive(active);
    }

    public void ToggleCanvasDialogue(bool active)
    {
        canvasDialogue.SetActive(active);
    }

    // Gegner registrieren
    public void RegisterEnemy(GameObject enemy)
    {
        if (!allEnemies.Contains(enemy))
        {
            allEnemies.Add(enemy);
        }
    }

    // Alle Gegner zurücksetzen
    public void ResetAllEnemies()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            enemy.transform.position = enemy.GetComponent<EnemyHealth>().GetStartPosition();
            enemy.GetComponent<EnemyHealth>().ResetHealth();
            enemy.SetActive(true);
        }
    }
}

