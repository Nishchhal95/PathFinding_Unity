using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingDijkstra : PathFinderBase
{
    [SerializeField] private List<Tile> tilesToExplore = new List<Tile>();
    [SerializeField] private List<Tile> tilesVisited = new List<Tile>();

    protected override void FindPath(Tile startTile, Tile endTile)
    {
        tilesToExplore = new List<Tile>();
        tilesVisited = new List<Tile>();

        startTile.Distance = 0;
        tilesToExplore.Add(startTile);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = GetTileWithLowestDistance(tilesToExplore);
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

                float newCostToNeighbor = currentTile.Distance + GetDistance(currentTile, neighbouringTile);
                if (newCostToNeighbor < neighbouringTile.Distance)
                {
                    neighbouringTile.SetParentTile(currentTile);
                    neighbouringTile.Distance = newCostToNeighbor;
                    tilesToExplore.Add(neighbouringTile);
                }

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

        startTile.Distance = 0;
        tilesToExplore.Add(startTile);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = GetTileWithLowestDistance(tilesToExplore);
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

                float newCostToNeighbor = currentTile.Distance + GetDistance(currentTile, neighbouringTile);
                if (newCostToNeighbor < neighbouringTile.Distance)
                {
                    neighbouringTile.SetParentTile(currentTile);
                    neighbouringTile.Distance = newCostToNeighbor;
                    tilesToExplore.Add(neighbouringTile);
                }

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

    private Tile GetTileWithLowestDistance(List<Tile> tileList)
    {
        Tile minTile = tileList[0];
        foreach (Tile tile in tileList)
        {
            if (tile.Distance < minTile.Distance)
            {
                minTile = tile;
            }
        }
        return minTile;
    }

    private float GetDistance(Tile tileA, Tile tileB)
    {
        //We are just one Unit apart for now.
        return 1;
    }
}
