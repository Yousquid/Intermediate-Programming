using System.Linq;
using UnityEngine;

public class ClickEventer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject firstObjectClicked;
    public GameObject secondObjectClicked;
    private Vector2Int firstObjectPos;
    private Vector2Int secondObjectPos;
    private GameObject objectToMove;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gridManager.ClearAllHightlights();
            DetectClickedObject();
        }

        HighlightSelectionGrid();
    }

    private void DetectClickedObject()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        Vector2Int gridPos = gridManager.GetGridPosition(worldPosition);

        if (hit.collider == null)
        {
            ClearAllClickedObjects();
            return;
        }

        GameObject clickedObject = hit.collider.gameObject;

        if (firstObjectClicked == null)
        {
            if (clickedObject.tag != "grid")
            {
                firstObjectClicked = clickedObject;
                firstObjectPos = gridPos;
            }
        }
        else if (secondObjectClicked == null && clickedObject != firstObjectClicked)
        {
            secondObjectClicked = clickedObject;
            secondObjectPos = gridPos;
            HandleClickInteractions();
        }
        else if (clickedObject == firstObjectClicked)
        {
            return;
        }
        else
        {
            firstObjectClicked = clickedObject;
            firstObjectPos = gridPos;
            secondObjectClicked = null;
        }
    }

    private void HandleClickInteractions()
    {
        if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "none" ||
            gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "grass" )
        {
            if (gridManager.grid[secondObjectPos.x, secondObjectPos.y].isOccupied &&
                gridManager.grid[secondObjectPos.x, secondObjectPos.y].objectType !=
                gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType)
            ClearAllClickedObjects();
            return;
        }

        if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "rabbit" &&
            gridManager.grid[firstObjectPos.x, firstObjectPos.y].action > 0)
        {
            if (ValidAnimalMove("rabbit", firstObjectPos, secondObjectPos) && 
                gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == 
                gridManager.grid[secondObjectPos.x, secondObjectPos.y].objectType)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (!gridManager.grid[firstObjectPos.x + x, firstObjectPos.y + y].isOccupied)
                        {
                            if (x != 0 && y != 0)
                            {
                                gridManager.SetCell(firstObjectPos.x + x, firstObjectPos.y + y, gridManager.rabbit, "rabbit");
                                gridManager.grid[firstObjectPos.x, firstObjectPos.y].action -= 1;
                                gridManager.grid[firstObjectPos.x + x, firstObjectPos.y + y].action--;
                                gridManager.grid[secondObjectPos.x, secondObjectPos.y].action--;
                                break;
                            }
                            
                        }
                    }
                    break;
                }
            }
            else if (ValidAnimalMove("rabbit", firstObjectPos, secondObjectPos))
            {
                objectToMove = firstObjectClicked;
                MoveObjectToGrid(secondObjectPos);
            }
        }

        ClearAllClickedObjects();
    }

    private void MoveObjectToGrid(Vector2Int targetPos)
    {
        if (objectToMove == null) return;

        gridManager.SetCell(targetPos.x, targetPos.y,
            gridManager.grid[firstObjectPos.x, firstObjectPos.y].content,
            gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType);

        gridManager.RemoveContent(firstObjectPos.x, firstObjectPos.y);
        gridManager.grid[targetPos.x, targetPos.y].action--;

        Destroy(firstObjectClicked);
        objectToMove = null;
    }

    private void ClearAllClickedObjects()
    {
        firstObjectClicked = null;
        secondObjectClicked = null;
    }

    private bool ValidAnimalMove(string animalType, Vector2Int start, Vector2Int target)
    {
        return GetValidMovePositions(animalType, start).Contains(target);
    }

    private Vector2Int[] GetValidMovePositions(string animalType, Vector2Int position)
    {
        if (animalType == "rabbit")
        {
            return new[]
            {
                new Vector2Int(position.x - 1, position.y),
                new Vector2Int(position.x + 1, position.y),
                new Vector2Int(position.x, position.y + 1),
                new Vector2Int(position.x, position.y - 1)
            };
        }
        return new Vector2Int[0];
    }

    private void HighlightSelectionGrid()
    {
        if (firstObjectClicked != null && gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "grass" &&
            gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "none")
        {
            foreach (Vector2Int pos in GetValidMovePositions(gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType, firstObjectPos))
            {
                if (gridManager.grid[pos.x, pos.y].objectType == gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType)
                {
                    gridManager.SetCellMateSign(pos);
                }
                else
                {
                    gridManager.SetCellHighlighted(pos);

                }
            }
        }
        else
        {
            gridManager.ClearAllHightlights();
        }
    }

}
