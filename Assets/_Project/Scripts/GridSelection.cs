using UnityEngine;
using UnityEngine.EventSystems;

public class GridSelection : MonoBehaviour
{
    [SerializeField] private LayerMask tileMask;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private PathFindingManager pathFindingManager;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMouseClicks();
    }

    private void HandleMouseClicks()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            HandleLeftMouseClick();
        }

        if(Input.GetMouseButtonDown(1))
        {
            HandleRightMouseClick();
        }
    }

    private void HandleLeftMouseClick()
    {
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200f, tileMask))
        {
            return;
        }

        Tile hitTile = hit.transform.GetComponent<Tile>();

        if (pathFindingManager.StartTile == null)
        {
            pathFindingManager.StartTile = hitTile;
        }
        else
        {
            pathFindingManager.EndTile = hitTile;
        }
    }

    private void HandleRightMouseClick()
    {
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200f, tileMask))
        {
            return;
        }

        Tile hitTile = hit.transform.GetComponent<Tile>();
        hitTile.ToggleIsWalkable();
    }
}
