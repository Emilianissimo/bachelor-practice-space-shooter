using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenesNamespace;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    /// <summary>
    /// Signal from button to load scene of game (start game)
    /// </summary>
    public void LoadGame()
    {
        SceneManager.LoadScene((int)Scenes.Game);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
