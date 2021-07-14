using System;
using Target;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private Vector3 _tile;
    private TargetManager _targetManager;
    private ScoreManager _scoreManager;
    
    private void Start()
    {
        _targetManager = TargetManager.Instance;
        _scoreManager = ScoreManager.Instance;
    }

    public void SetTile(Vector3 tile)
    {
        _tile = tile;
    }

    public Vector3 GetTile()
    {
        return _tile;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _targetManager.RemoveTarget(this);
        _scoreManager.AddScore(Config.PerTargetScoreAmount);
    }
}
