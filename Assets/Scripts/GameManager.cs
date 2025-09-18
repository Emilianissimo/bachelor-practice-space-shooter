using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScenesNamespace;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    /// <summary>
    /// Operate user input
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene((int)Scenes.Game);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }
    }

    /// <summary>
    /// GameOver setter. True only.
    /// </summary>
    public void GameOver()
    {
        _isGameOver = true;
    }
}
