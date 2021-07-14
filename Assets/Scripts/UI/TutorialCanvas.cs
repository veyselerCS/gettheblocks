using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvas : MonoBehaviour
{
    [SerializeField] private Button GoButton;

    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
        GoButton.onClick.AddListener(() =>
        {
            _gameManager.StartGame();
            Hide();
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
