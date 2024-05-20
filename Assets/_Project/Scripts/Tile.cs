using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileState state = TileState.Default;

    private Renderer tileRenderer;
    private MaterialPropertyBlock materialPropertyBlock;

    [SerializeField] private GameObject arrowGO;
    [field: SerializeField] public Vector2Int tileArrayIndex { get; private set; }
    [field: SerializeField] public Tile parentTile { get; private set; }
    [field: SerializeField] public bool IsWalkable { get; private set; }
    [field: SerializeField] public float Heuristic { get; set; }

    private Color defaultColor = Color.white;
    private Color startColor = Color.red;
    private Color endColor = Color.green;
    private Color processingColor = Color.yellow;
    private Color blockedColor = Color.black;
    private Color pathColor = Color.cyan;

    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        tileRenderer = GetComponentInChildren<Renderer>();
    }

    public void SetData(Vector2Int tileArrayIndex, bool isWalkable = true)
    {
        IsWalkable = isWalkable;
        this.tileArrayIndex = tileArrayIndex;
        ChangeState(IsWalkable ? TileState.Default : TileState.Blocked);
    }
    public void ChangeState(TileState stateToChange)
    {
        if (state == stateToChange)
        {
            return;
        }

        state = stateToChange;

        Color colorToChange = defaultColor;
        switch (stateToChange)
        {
            case TileState.Default:
                break;
            case TileState.Start:
                colorToChange = startColor;
                break;
            case TileState.End:
                colorToChange = endColor;
                break;
            case TileState.Processing:
                colorToChange = processingColor;
                break;
            case TileState.Blocked:
                colorToChange = blockedColor;
                break;
            case TileState.Path:
                colorToChange = pathColor;
                break;
        }

        SetColor(colorToChange);
    }

    public void ToggleIsWalkable()
    {
        IsWalkable = !IsWalkable;
        ChangeState(IsWalkable ? TileState.Default : TileState.Blocked);
    }

    public void SetParentTile(Tile parentTile, bool isDebug = false)
    {
        this.parentTile = parentTile;
        SetArrowState(isDebug);
    }

    public void ResetTile()
    {
        parentTile = null;
        IsWalkable = true;
        ChangeState(TileState.Default);
        arrowGO.SetActive(false);
    }

    private void SetArrowState(bool state)
    {
        arrowGO.transform.LookAt(parentTile.transform.position);
        arrowGO.SetActive(state);
    }

    private void SetColor(Color color)
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_BaseColor", color);
        tileRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}

public enum TileState
{
    Default = 0,
    Start = 1,
    End = 2,
    Processing = 3,
    Blocked = 4,
    Path = 5
}
