using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI CurrentScoreText;
        [SerializeField] private TextMeshProUGUI MaxScoreText;

        private ScoreManager _scoreManager;

        private void Start()
        {
            _scoreManager = ScoreManager.Instance;
            
            //init the scores
            CurrentScoreText.text = "Current : " + _scoreManager.GetCurrentScore();
            MaxScoreText.text = "Max : " + _scoreManager.GetMaxScore();

            _scoreManager.SubscribeScoreChanged(OnScoreChanged);
            _scoreManager.SubscribeMaxScoreChanged(OnMaxScoreChanged);
        }

        private void OnScoreChanged()
        {
            CurrentScoreText.text = "Current : " + _scoreManager.GetCurrentScore();
        }

        private void OnMaxScoreChanged()
        {
            MaxScoreText.text = "Max : " + _scoreManager.GetMaxScore();
        }
    }
}