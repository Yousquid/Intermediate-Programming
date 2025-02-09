using System.Linq;
using UnityEngine;

public class ClickEventer : MonoBehaviour
{
    public GridManager gridManager;
    public BuildingManager buildingManager;
    public GameObject firstObjectClicked;
    public GameObject secondObjectClicked;
    public Vector2Int firstObjectPos;
    public Vector2Int secondObjectPos;
    private GameObject objectToMove;
    private UIManager UImanager;
    public LayerMask layerMask;

    private void Start()
    {
        UImanager = GetComponent<UIManager>();
    }

    void Update()
    {
        ClickedObjectUI();

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
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition,layerMask);
        Vector2Int gridPos = gridManager.GetGridPosition(worldPosition);

        Vector2 buttonScreenPos = RectTransformUtility.WorldToScreenPoint(null, UImanager.animalSpecialAbilityButton.transform.position);

        // Get the RectTransform
        RectTransform buttonRect = UImanager.animalSpecialAbilityButton.GetComponent<RectTransform>();

        // Calculate the button bounds in screen space
        float left = buttonScreenPos.x - (buttonRect.rect.width * 0.5f);
        float right = buttonScreenPos.x + (buttonRect.rect.width * 0.5f);
        float top = buttonScreenPos.y + (buttonRect.rect.height * 0.5f);
        float bottom = buttonScreenPos.y - (buttonRect.rect.height * 0.5f);

        // Now check if the hit point is within the button bounds
     

        if (hitCollider == null)
        {
           
            
                ClearAllClickedObjects();
                return;
            
            
        }

        GameObject clickedObject = hitCollider.gameObject;

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
            //SPAWN A NEW RABBIT IF CAN MATE AND WALK ONTO ANOTHER RABBIT
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
            //MOVE THE RABBIT
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

        print(gridManager.grid[3, 3].backgroundType);
        //Teleport to another hole if there is one
        if (gridManager.grid[targetPos.x, targetPos.y].backgroundType == "hole")
        {
            if (buildingManager.GiveAnotherHolePosition(targetPos) != Vector2Int.zero)
            {
                string targetBackgrouObjectType = gridManager.grid[targetPos.x, targetPos.y].backgroundType;
                Vector2Int holeTarget = buildingManager.GiveAnotherHolePosition(targetPos);
                gridManager.SetCell(holeTarget.x, holeTarget.y,
                gridManager.grid[firstObjectPos.x, firstObjectPos.y].content,
                gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType, targetBackgrouObjectType);
            }
        }
        //Move to the new cell by setting the content of the new grid
        else
        {
            string targetBackgrouObjectType = gridManager.grid[targetPos.x, targetPos.y].backgroundType;
            gridManager.SetCell(targetPos.x, targetPos.y,
               gridManager.grid[firstObjectPos.x, firstObjectPos.y].content,
               gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType, targetBackgrouObjectType);
        }
        //Remove the moved object in the orginal grid and induce the object's action
        string firstPosBackgroundType = gridManager.grid[firstObjectPos.x, firstObjectPos.y].backgroundType;
        gridManager.RemoveContent(firstObjectPos.x, firstObjectPos.y, firstPosBackgroundType);
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

    void ClickedObjectUI()
    {
        if (firstObjectClicked != null)
        {
            UImanager.ShowAnimalUI(gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType);
        }
        else if (firstObjectClicked == null)
        {
            UImanager.HideAnimalUI();
        }

    }
}
