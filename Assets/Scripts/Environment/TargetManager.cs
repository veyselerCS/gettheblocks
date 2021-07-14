using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Target
{
    public class TargetManager : MonoBehaviour
    {
        public static TargetManager Instance;
        
        [SerializeField] private TargetController TargetPrefab;
        [SerializeField] private Transform TargetsParentTransform;

        private Stack<TargetController> _availableTargets = new Stack<TargetController>();
        private List<TargetController> _unavailableTargets = new List<TargetController>();
 
        private BoardManager _boardManager;
        private GameManager _gameManager;
        
        public int OnBoardTargetCount;
        private Vector3 _randomPosCache;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _boardManager = BoardManager.Instance;
            _gameManager = GameManager.Instance;
            
            _gameManager.SubscribeToRestart(OnRestart);
            OnBoardTargetCount = 0; 
            CreateTargetPool(Config.MaxTargetCountOnBoard);
        }

        private void CreateTargetPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var target = Instantiate(TargetPrefab, TargetsParentTransform);
                target.gameObject.SetActive(false);
                _availableTargets.Push(target);
            }
        }
        
        public void CreateTarget()
        {
            TargetController target;
            
            //check the pool first
            if (_availableTargets.Count == 0)
            {
                target = Instantiate(TargetPrefab, TargetsParentTransform);
            }
            else
            {
                target = _availableTargets.Pop();
            }
            
            var tile = _boardManager.GetRandomAvailableTail();
            RandomizeXZPosition(tile);

            target.gameObject.SetActive(true);
            target.transform.position = _randomPosCache;
            target.SetTile(tile);
            
            _unavailableTargets.Add(target);
            OnBoardTargetCount++;
        }

        public void RemoveTarget(TargetController target)
        {
            target.gameObject.SetActive(false);

            _boardManager.ReleaseTile(target.GetTile());
            _availableTargets.Push(target);
            _unavailableTargets.Remove(target);
            OnBoardTargetCount--;
        }

        private void RandomizeXZPosition(Vector3 pos)
        {
            _randomPosCache.x = pos.x + Random.Range(0f, Config.TargetPositionRandomizerOffset);
            _randomPosCache.y = pos.y;
            _randomPosCache.z = pos.z + Random.Range(0f, Config.TargetPositionRandomizerOffset);
        }
        
        private void OnRestart()
        {
            int removeCount = _unavailableTargets.Count;
            for(int i = 0; i < removeCount; i++)
            {
                //remove the last element for efficiency
                RemoveTarget(_unavailableTargets[_unavailableTargets.Count - 1]);
            }
        }
    }
}