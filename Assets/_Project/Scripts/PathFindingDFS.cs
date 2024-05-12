using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingDFS : PathFinderBase
{
    [SerializeField] private Stack<Tile> tilesToExplore = new Stack<Tile>();
    [SerializeField] private List<Tile> tilesVisited = new List<Tile>();

    protected override void FindPath(Tile startTile, Tile endTile)
    {
        tilesToExplore = new Stack<Tile>();
        tilesVisited = new List<Tile>();


        tilesToExplore.Push(startTile);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = tilesToExplore.Pop();

            if (currentTile == endTile)
            {
                break;
            }

            bool isNeighbouringTileTheEndTile = false;
            List<Tile> neighbouringTiles = gridManager.FindNeighbours(currentTile);
            foreach (Tile neighbouringTile in neighbouringTiles)
            {
                if (!tilesVisited.Contains(neighbouringTile))
                {
                    tilesVisited.Add(neighbouringTile);
                    neighbouringTile.SetParentTile(currentTile);

                    tilesToExplore.Push(neighbouringTile);
                }

                if (neighbouringTile == endTile)
                {
                    isNeighbouringTileTheEndTile = true;
                    break;
                }
            }

            if (isNeighbouringTileTheEndTile)
            {
                break;
            }

            tilesVisited.Add(currentTile);
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
        tilesToExplore = new Stack<Tile>();
        tilesVisited = new List<Tile>();

        tilesToExplore.Push(startTile);

        while (tilesToExplore.Count > 0)
        {
            Tile currentTile = tilesToExplore.Pop();

            if (currentTile == endTile)
            {
                yield break;
            }

            List<Tile> neighbouringTiles = gridManager.FindNeighbours(currentTile);
            foreach (Tile neighbouringTile in neighbouringTiles)
            {
                if (!tilesVisited.Contains(neighbouringTile))
                {
                    tilesVisited.Add(neighbouringTile);
                    neighbouringTile.SetParentTile(currentTile);

                    tilesToExplore.Push(neighbouringTile);

                    if (neighbouringTile == endTile)
                    {
                        yield break;
                    }

                    neighbouringTile.ChangeState(TileState.Processing);
                }
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
}
