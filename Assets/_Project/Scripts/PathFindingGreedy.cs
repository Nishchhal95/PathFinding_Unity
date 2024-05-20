using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingGreedy : PathFinderBase
{
    [SerializeField] private List<Tile> tilesToExplore = new List<Tile>();
    [SerializeField] private List<Tile> tilesVisited = new List<Tile>();

    protected override void FindPath(Tile startTile, Tile endTile)
    {
        tilesToExplore = new List<Tile>();
        tilesVisited = new List<Tile>();

        startTile.Heuristic = GetHeuristic(startTile, endTile);
        tilesToExplore.Add(startTile);

        while (tilesToExplore.Count > 0)
        {
            tilesToExplore.Sort((tileA, tileB) => tileA.Heuristic.CompareTo(tileB.Heuristic));
            Tile currentTile = tilesToExplore[0];
            tilesToExplore.Remove(currentTile);
            tilesVisited.Add(currentTile);

            if (currentTile == endTile)
            {
                return;
            }

            List<Tile> neighbouringTiles = gridManager.FindNeighbours(currentTile);
            foreach (Tile neighbouringTile in neighbouringTiles)
            {
                if (tilesVisited.Contains(neighbouringTile))
                {
                    continue;
                }

                neighbouringTile.SetParentTile(currentTile);
                neighbouringTile.Heuristic = GetHeuristic(neighbouringTile, endTile);
                tilesToExplore.Add(neighbouringTile);

                if (neighbouringTile == endTile)
                {
                    return;
                }
            }
        }
    }

    protected override void CreatePath(Tile startTile, Tile endTile)
    {
        Tile currentTile = endTile;
        while (currentTile.parentTile != null || currentTile != startTile)
        {
            if (currentTile != endTile && currentTile != startTile)
            {
                currentTile.ChangeState(TileState.Path);
            }
            currentTile = currentTile.parentTile;
        }
    }

    protected override IEnumerator FindPathRoutine(Tile startTile, Tile endTile, float stepDelay = 0.1f)
    {
        tilesToExplore = new List<Tile>();
        tilesVisited = new List<Tile>();

        startTile.Heuristic = GetHeuristic(startTile, endTile);
        tilesToExplore.Add(startTile);

        while (tilesToExplore.Count > 0)
        {
            tilesToExplore.Sort((tileA, tileB) => tileA.Heuristic.CompareTo(tileB.Heuristic));
            Tile currentTile = tilesToExplore[0];
            tilesToExplore.Remove(currentTile);
            tilesVisited.Add(currentTile);

            if (currentTile == endTile)
            {
                yield break;
            }

            List<Tile> neighbouringTiles = gridManager.FindNeighbours(currentTile);
            foreach (Tile neighbouringTile in neighbouringTiles)
            {
                if (tilesVisited.Contains(neighbouringTile))
                {
                    continue;
                }

                neighbouringTile.SetParentTile(currentTile);
                neighbouringTile.Heuristic = GetHeuristic(neighbouringTile, endTile);
                tilesToExplore.Add(neighbouringTile);

                if (neighbouringTile == endTile)
                {
                    yield break;
                }

                neighbouringTile.ChangeState(TileState.Processing);
            }

            yield return new WaitForSecondsRealtime(stepDelay);

            tilesVisited.Add(currentTile);
        }
    }

    protected override IEnumerator CreatePathRoutine(Tile startTile, Tile endTile, float stepDelay = 0.1f)
    {
        Tile currentTile = endTile;
        while (currentTile.parentTile != null || currentTile != startTile)
        {
            yield return new WaitForSecondsRealtime(stepDelay);
            if (currentTile != endTile && currentTile != startTile)
            {
                currentTile.ChangeState(TileState.Path);
            }
            currentTile = currentTile.parentTile;
        }
    }

    private float GetHeuristic(Tile tileA, Tile tileB)
    {
        //Manhattan distance
        return Mathf.Abs(tileA.tileArrayIndex.x - tileB.tileArrayIndex.x) + 
            Mathf.Abs(tileA.tileArrayIndex.y - tileB.tileArrayIndex.y);
    }
}
