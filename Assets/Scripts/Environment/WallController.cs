using System;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        _gameManager.OnPlayerDeath();
    }
}
