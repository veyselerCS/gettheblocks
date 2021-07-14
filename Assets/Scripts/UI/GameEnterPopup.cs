using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEnterPopup : MonoBehaviour
    {
        [SerializeField] private Button PlayButton;
        [SerializeField] private TextMeshProUGUI PlayButtonText;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private Image Background;

        private GameManager _gameManager;
        private ScoreManager _scoreManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _scoreManager = ScoreManager.Instance;
            PlayButton.onClick.AddListener(OnClickPlayButton);
            
            ScoreText.text = "Max : " + _scoreManager.GetMaxScore();
        }
    
        public void Show()
        {
            gameObject.SetActive(true);

            if (_gameManager.GameState == GameState.Restart)
            {
                Background.color = Color.red;
                PlayButtonText.text = "Play Again";
                ScoreText.text = "Current : " + _scoreManager.GetCurrentScore();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void OnClickPlayButton()
        {
            if (_gameManager.GameState == GameState.Beginning)
            {
                _gameManager.StartGame();
            }
            else
            {
                _gameManager.RestartGame();
            }

            Hide();
        }
    }
}