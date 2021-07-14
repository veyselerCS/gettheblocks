using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
public class DigitalControllerUI : MonoBehaviour
{
    [SerializeField] private Transform controllerCenterTransform;
    [SerializeField] private RectTransform ControllerRectTransform;
    [SerializeField] private Transform ArrowTransform;

    private PlayerManager _playerManager;
    private GameManager _gameManager;
    
    private Coroutine _touchListener;
    private Coroutine _getDirectionCoroutine;

    private bool _touchState;
    private Vector3 _controllerCenterPosition;
    
    private void Start()
    {
        _playerManager = PlayerManager.Instance;
        _gameManager = GameManager.Instance;

        var controllerPos = controllerCenterTransform.position;
        var controllerSize = ControllerRectTransform.sizeDelta;
        
        _controllerCenterPosition = new Vector3(controllerPos.x, controllerPos.y + (controllerSize.y / 2), controllerPos.z);
        _gameManager.SubscribeToRestart(OnRestart);
        StartCoroutine(GetDirection());
    }

    private void OnRestart()
    {
        ArrowTransform.rotation = Quaternion.identity;
    }
    
    private IEnumerator GetDirection()
    {
        while (true)
        {
            if(_touchState)
            {
                var dir =  Input.mousePosition - _controllerCenterPosition;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                var angleWithOffset = angle - 90;
                
                ArrowTransform.DORotate(new Vector3(0, 0, angleWithOffset), 0.2f);
                _playerManager.MoveTowards(new Vector3(0, 0, -1 * angleWithOffset));
            }
        
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(ControllerRectTransform, Input.mousePosition))
        {
            _touchState = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _touchState = false;
        }
    }
}
