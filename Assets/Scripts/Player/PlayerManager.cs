using System;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    [SerializeField] private Transform PlayerTransform;

    private GameManager _gameManager;
    private Vector3 _direction;
    private Vector3 _initialPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SubscribeToRestart(OnRestart);
        _initialPosition = PlayerTransform.position;
    }

    private void OnRestart()
    {
        PlayerTransform.position = _initialPosition;
        PlayerTransform.rotation = Quaternion.identity;
    }
    
    public void MoveTowards(Vector3 direction)
    {
        PlayerTransform.DORotate(new Vector3(0, direction.z, 0), 0.1f);
        PlayerTransform.position += PlayerTransform.forward * (Time.deltaTime * Config.MoveSpeed);
    }
}
