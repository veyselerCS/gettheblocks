using System;
using System.Collections;
using Core;
using Target;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameEnterPopup _gameEnterPopup;

    [SerializeField] private TutorialCanvas _tutorialCanvas;
    
    private TargetManager _targetManager;
    public GameState GameState;
    private WaitForSeconds _waitForNextTargetCreation;
    private Coroutine _checkTargetsCoroutine;
    private event Action RestartEvent;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _targetManager = TargetManager.Instance;
        _waitForNextTargetCreation = new WaitForSeconds(Config.SecondsBeforeTargetCreation);

        GameState = GameState.Beginning;
        if (!PlayerPrefs.HasKey("TutorialPlayed"))
        {
            PlayerPrefs.SetInt("TutorialPlayed", 1);
            _gameEnterPopup.Hide();
        }
        else
        {
            _tutorialCanvas.Hide();
        }
    }

    public void StartGame()
    {
        GameState = GameState.Play;
        _checkTargetsCoroutine = StartCoroutine(CheckTargets());
    }

    public void OnPlayerDeath()
    {
        StopCoroutine(_checkTargetsCoroutine);
        GameState = GameState.Restart;
        _gameEnterPopup.Show();
    }
    
    public void RestartGame()
    {
        FireRestartEvent();
        StartGame();
    }

    private IEnumerator CheckTargets()
    {
        while (true)
        {
            if ((GameState == GameState.Play) && _targetManager.OnBoardTargetCount < Config.MaxTargetCountOnBoard)
            {
                _targetManager.CreateTarget();
            }

            yield return _waitForNextTargetCreation;
        }
    }

    public void FireRestartEvent()
    {
        if (RestartEvent != null)
        {
            RestartEvent();
        }
    }

    public void SubscribeToRestart(Action action)
    {
        RestartEvent += action;
    }
}