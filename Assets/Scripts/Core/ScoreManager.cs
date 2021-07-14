using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    public event Action ScoreChangedEvent;
    public event Action MaxScoreChangedEvent;

    private GameManager _gameManager;
    private int _currentMaxScore;
    private int _currentScore;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (PlayerPrefs.HasKey("MaxScore"))
        {
            _currentMaxScore = PlayerPrefs.GetInt("MaxScore");
        }
        else
        {
            _currentMaxScore = 0;
            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SubscribeToRestart(OnRestart);
    }

    public void AddScore(int amount)
    {
        _currentScore += amount;
        CheckMaxScore();
        FireScoreChanged();
    }

    public void ResetScore()
    {
        _currentScore = 0;
        FireScoreChanged();
    }

    private void CheckMaxScore()
    {
        if (_currentScore > _currentMaxScore)
        {
            _currentMaxScore = _currentScore;
            PlayerPrefs.SetInt("MaxScore", _currentMaxScore);
            FireMaxScoreChanged();
        }
    }

    private void OnRestart()
    {
        ResetScore();
    }

    public int GetCurrentScore()
    {
        return _currentScore;
    }

    public int GetMaxScore()
    {
        return _currentMaxScore;
    }
    
    private void FireScoreChanged()
    {
        if (ScoreChangedEvent != null)
        {
            ScoreChangedEvent();
        }
    }

    private void FireMaxScoreChanged()
    {
        if (MaxScoreChangedEvent != null)
        {
            MaxScoreChangedEvent();
        }
    }
    
    public void SubscribeScoreChanged(Action action)
    {
        ScoreChangedEvent += action;
    }
    
    public void SubscribeMaxScoreChanged(Action action)
    {
        MaxScoreChangedEvent += action;
    }
}