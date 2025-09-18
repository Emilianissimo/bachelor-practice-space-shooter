using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Game
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    /// <summary>
    /// Operate user input to restart the Game (Scene)
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene((int)Scenes.Game);
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
