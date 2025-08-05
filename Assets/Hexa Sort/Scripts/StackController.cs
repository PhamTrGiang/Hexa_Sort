using System;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask hexagonLayerMark;
    [SerializeField] private LayerMask gridHexagonLayerMark;
    [SerializeField] private LayerMask groundLayerMark;
    private HexStack currentStack;
    private Vector3 currentStackInitialPos;

    [Header("Data")]
    private GridCell targetCell;

    [Header("Action")]
    public static Action<GridCell> onStackPlaced;

    private void Update()
    {
        if (GameManager.Instance.currentState != State.GAME)
            return;
        ManageControll();
    }

    private void ManageControll()
    {
        if (Input.GetMouseButtonDown(0))
            ManageMouseDown();
        else if (Input.GetMouseButton(0) && currentStack != null)
            ManageMouseDrag();
        else if (Input.GetMouseButtonUp(0) && currentStack != null)
            ManageMouseUp();
    }


    private void ManageMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMark);

        if (hit.collider == null)
        {
            return;
        }
        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitialPos = currentStack.transform.position;
    }


    private void ManageMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMark);

        if (hit.collider == null)
            DraggingAboveGround();
        else
            DragingAboveGridCell(hit);
    }


    private void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMark);

        if (hit.collider == null)
        {
            return;
        }

        Vector3 newPos = hit.point;
        newPos.y = 1;

        Vector3 currenStackTargetPos = newPos;

        currentStack.transform.position = Vector3.MoveTowards(
                        currentStack.transform.position,
                        currenStackTargetPos,
                        Time.deltaTime * 30);

        targetCell = null;
    }

    private void DragingAboveGridCell(RaycastHit hit)
    {
        GridCell gridCell = hit.collider.GetComponent<GridCell>();

        if (gridCell.IsOccupied)
            DraggingAboveGround();
        else
            DraggingAboveNonOccupiedGridCell(gridCell);
    }

    private void DraggingAboveNonOccupiedGridCell(GridCell gridCell)
    {

        Vector3 newPos = gridCell.transform.position;
        newPos.y = 1;

        Vector3 currenStackTargetPos = newPos;

        currentStack.transform.position = Vector3.MoveTowards(
                        currentStack.transform.position,
                        currenStackTargetPos,
                        Time.deltaTime * 30);

        targetCell = gridCell;
    }

    private void ManageMouseUp()
    {
        if (targetCell == null)
        {
            currentStack.transform.position = currentStackInitialPos;
            currentStack = null;
            return;
        }

        Vector3 targetPos = targetCell.transform.position;
        targetPos.y = .1f;

        currentStack.transform.position = targetPos;
        currentStack.transform.SetParent(targetCell.transform);
        currentStack.Place();

        targetCell.AssignStack(currentStack);

        onStackPlaced?.Invoke(targetCell);

        targetCell = null;
        currentStack = null;
    }

    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
