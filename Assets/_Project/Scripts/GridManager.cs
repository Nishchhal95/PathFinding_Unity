using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int length = 0;
    [SerializeField] private int width = 0;
    [SerializeField] private float cameraPadding = 1f;
    [SerializeField] private Transform tileMapParent;
    [SerializeField] private Tile[,] tileMap;
    [SerializeField] private Camera mainCamera;

    [Space]
    [Header("Tile")]
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private float tileLength = 0;
    [SerializeField] private float tileWidth = 0;

    private void Start()
    {
        SetupCamera();
        GenerateGrid();
    }

    private void SetupCamera()
    {
        float xPos = width % 2 == 0 ? ((width / 2) * tileWidth) - (tileWidth / 2) : ((width - 1) / 2) * tileWidth;
        float zPos = length % 2 == 0 ? ((length / 2) * tileLength) - (tileLength / 2) : ((length - 1) / 2) * tileLength;
        Vector3 centerPointOfGrid = new Vector3 (xPos, 1, zPos);

        float aspectRatio = (float)Screen.width / Screen.height;
        float orthoSizeWidth = (tileWidth * width) / aspectRatio / 2f;
        float orthoSizeLength = (tileLength * length) / 2f;

        mainCamera.orthographicSize = Mathf.Max(orthoSizeWidth, orthoSizeLength);
        mainCamera.orthographicSize += cameraPadding;
        mainCamera.transform.position = centerPointOfGrid;
    }

    private void GenerateGrid()
    {
        tileMap = new Tile[width, length];
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 tilePosition = new Vector3((x * tileWidth), 0, (y * tileLength));
                Vector2Int tileArrayIndex = new Vector2Int(x, y);

                Tile tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, tileMapParent);
                tile.transform.name = $"Tile {tileArrayIndex}";
                tile.SetData(tileArrayIndex);
                AddTile(tile, tileArrayIndex);
            }
        }
    }

    private void AddTile(Tile tile, Vector2Int tileArrayIndex)
    {
        if (tile == null)
        {
            Debug.LogError("Cannot add a null tile");
        }

        if (!IsIndexInRange(tileArrayIndex))
        {
            Debug.LogError($"Cannot add tile as the index {tileArrayIndex} " +
                $"is out of Bounds, widht = {width} and length = {length}");
        }

        tileMap[tileArrayIndex.x, tileArrayIndex.y] = tile;
    }

    public Tile GetTileFromIndex(Vector2Int tileIndex)
    {
        if (!IsIndexInRange(tileIndex))
        {
            return null;
        }

        return tileMap[tileIndex.x, tileIndex.y];
    }

    public Vector2Int GetIndexFromTile(Tile tile)
    {
        if (tile == null)
        {
            Debug.LogError("Cannot add a null tile");
        }

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (tileMap[x, y] == tile)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        Debug.LogError("Cannot find Index for the given Tile");
        return new Vector2Int();
    }

    public List<Tile> FindNeighbours(Tile tile)
    {
        List<Tile> neighbouringTiles = new List<Tile>();

        Vector2Int tileIndex = GetIndexFromTile(tile);
        Vector2Int[] searchingOrder = new Vector2Int[4]
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
        };

        for (int i = 0; i < searchingOrder.Length; i++)
        {
            Tile neighbouringTile = GetTileFromIndex(tileIndex + searchingOrder[i]);
            if (neighbouringTile != null && neighbouringTile.IsWalkable)
            {
                neighbouringTiles.Add(neighbouringTile);
            }
        }

        return neighbouringTiles;
    }

    public void ResetAllTiles()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tileMap[x, y].ResetTile();
            }
        }
    }

    public void ResetAllTilesExceptBlocked()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(tileMap[x, y].IsWalkable)
                {
                    tileMap[x, y].ResetTile();
                }
            }
        }
    }

    public void ClearPath()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!tileMap[x, y].IsWalkable)
                {
                    continue;
                }

                tileMap[x, y].ResetTile();
            }
        }
    }

    public void ClearLevel()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tileMap[x, y].ResetTile();
            }
        }
    }

    public void ClearProcessedNodes()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(tileMap[x, y].state == TileState.Processing)
                {
                    tileMap[x, y].ResetTile();
                }
            }
        }
    }

    private bool IsIndexInRange(Vector2Int tileArrayIndex)
    {
        if (tileArrayIndex.x < 0 || tileArrayIndex.y < 0)
        {
            return false;
        }

        if (tileArrayIndex.x >= width || tileArrayIndex.y >= length)
        {
            return false;
        }

        return true;
    }

}
