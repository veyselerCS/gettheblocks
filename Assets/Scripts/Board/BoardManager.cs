using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    
    [SerializeField] private Transform BoardTopRightTransform;
    [SerializeField] private Transform BoardBottomLeftTransform;

    [SerializeField] private List<Vector3> TileCenterPositions;
    
    [SerializeField] private int RowCount;
    [SerializeField] private int ColumnCount;

    private GameManager _gameManager;
    //integers in these lists coressponds to index in TileCenterPositions array
    private List<Vector3> _availableTilePositions = new List<Vector3>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        FillAvailableTilePositions();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SubscribeToRestart(OnRestart);
    }

    private void FillAvailableTilePositions()
    {
        //in the beginning all tiles are available
        for (int i = 0; i < TileCenterPositions.Count; i++)
        {
            _availableTilePositions.Add(TileCenterPositions[i]);
        }
    }

    public Vector3 GetRandomAvailableTail()
    {
        var randomIndex = Random.Range(0, _availableTilePositions.Count);
        var tileToSetUnavailable = _availableTilePositions[randomIndex];
        
        _availableTilePositions.RemoveAt(randomIndex);
        
        return tileToSetUnavailable;
    }

    public void ReleaseTile(Vector3 tile)
    {
        _availableTilePositions.Add(tile);
    }

    public void OnRestart()
    {
        _availableTilePositions.Clear();
        FillAvailableTilePositions();
    }
    
    [ContextMenu("Create Board Grids")]
    private void CreateBoardGrids()
    {
        //get positions to cache
        var bottomLeftPos = BoardBottomLeftTransform.position;
        var topRightPos = BoardTopRightTransform.position;
        
        var bottomLeftX = bottomLeftPos.x;
        var bottomLeftZ = bottomLeftPos.z;
        var TopRightX = topRightPos.x;
        var TopRightZ = topRightPos.z;
        
        //calculate offsets they are seperated by columns - 1 lines
        var xOffset = (TopRightX - bottomLeftX) / (ColumnCount - 1);
        var zOffset = (TopRightZ - bottomLeftZ) / (RowCount - 1);
        
        //in case we want to run the method again
        TileCenterPositions.Clear();
        
        //get the positions
        for (int row = 0; row < RowCount; row++)
        {
            for (int col = 0; col < ColumnCount; col++)
            {
                var newX = bottomLeftPos.x + xOffset * col;
                var newZ = bottomLeftPos.z + zOffset * row;
                Vector3 newTilePos = new Vector3(newX, 0.5f, newZ);
                TileCenterPositions.Add(newTilePos);
            }
        }
    }
}
