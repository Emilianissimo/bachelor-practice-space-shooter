using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreNumberText;

    [SerializeField]
    private Image _LivesImage;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Canvas _gameOverCanvas;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null) Debug.LogWarning("Game_Manager not found!");
    }

    /// <summary>
    /// Public setter to update our score in UI
    /// </summary>
    /// <param name="value">New score amount</param>
    public void SetScore(int value)
    {
        _scoreNumberText.text = value.ToString();
    }

    /// <summary>
    /// Public setter to update player lives in UI
    /// </summary>
    /// <param name="value">Int in range of 0-3</param>
    public void SetLives(int value)
    {
        _LivesImage.sprite = _liveSprites[value];
    }

    /// <summary>
    /// Public setter to show/hide game over canvas
    /// Calls GameManager.GameOver on gameover
    /// </summary>
    /// <param name="value">Bool</param>
    public void SetGameOverState(bool value)
    {
        _gameOverCanvas.gameObject.SetActive(value);
        if (value) _gameManager.GameOver();
    }
}
