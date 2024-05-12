using System.Collections;
using UnityEngine;

public abstract class PathFinderBase : MonoBehaviour
{
    [SerializeField] protected GridManager gridManager;

    public void StartPathFinding(Tile startTile, Tile endTile)
    {
        FindPath(startTile, endTile);
        CreatePath(startTile, endTile);
    }

    public void StartPathFindingDebugMode(Tile startTile, Tile endTile)
    {
        gridManager.ClearProcessedNodes();
        StartCoroutine(StartPathFindingDebugModeRoutine(startTile, endTile));
    }

    protected IEnumerator StartPathFindingDebugModeRoutine(Tile startTile, Tile endTile)
    {
        yield return StartCoroutine(FindPathRoutine(startTile, endTile));
        yield return StartCoroutine(CreatePathRoutine(startTile, endTile));
    }

    protected abstract void FindPath(Tile startTile, Tile endTile);
    protected abstract void CreatePath(Tile startTile, Tile endTile);

    protected abstract IEnumerator FindPathRoutine(Tile startTile, Tile endTile, float stepDelay = 0.1f);
    protected abstract IEnumerator CreatePathRoutine(Tile startTile, Tile endTile, float stepDelay = 0.1f);
}
