using System.Collections.Generic;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    [SerializeField] private List<PathFinderAlgoToConcreteObjectMap> pathFinderMap = new List<PathFinderAlgoToConcreteObjectMap>();
    public Tile StartTile 
    {
        get
        {
            return startTile;
        }
        set
        {
            startTile = value;
            startTile?.ChangeState(TileState.Start);
        }
    }
    public Tile EndTile
    {
        get
        {
            return endTile;
        }
        set
        {
            endTile = value;
            endTile?.ChangeState(TileState.End);
        }
    }
    [field: SerializeField] public bool IsDebug { get; set; }
    [field: SerializeField] public PathFindingAlgorithm PathFindingAlgorithm { get; set; }

    private Tile startTile;
    private Tile endTile;

    public void StartPathFinding()
    {
        foreach (PathFinderAlgoToConcreteObjectMap pathFinderItem in pathFinderMap)
        {
            if(pathFinderItem.PathFindingAlgorithm == PathFindingAlgorithm)
            {
                if(IsDebug)
                {
                    pathFinderItem.pathFinder.StartPathFindingDebugMode(StartTile, EndTile);
                }
                else
                {
                    pathFinderItem.pathFinder.StartPathFinding(StartTile, EndTile);
                }
            }
        }
    }
}

[System.Serializable]
public class PathFinderAlgoToConcreteObjectMap
{
    public PathFinderBase pathFinder;
    public PathFindingAlgorithm PathFindingAlgorithm;
}
