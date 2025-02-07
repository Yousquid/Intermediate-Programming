using System.Linq;
using UnityEngine;

public class ClickEventer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject firstObjectClicked;
    public GameObject secondObjectClicked;
    public Vector2Int firstObjectPos;
    public Vector2Int secondObjectPos;

    public GameObject objectToMove;
    void Update()
    {
        if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "none")
        {
            ClearAllClickedObjects();
        }

        if (Input.GetMouseButtonDown(0))
        {
            gridManager.ClearAllHightlights();
            ClickedObjectDetection();
           
        }
        SelectionGridHighlight();
    }

    public void ClickedObjectDetection()
    {
        
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            Vector3 worldPositionVector2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPositionVector2.z = 0; // Ignore depth
            
            if (hit.collider != null)
            {
                if (secondObjectClicked == null && firstObjectClicked == null)
                {
                    firstObjectClicked = hit.collider.gameObject;

                    if (firstObjectClicked.tag != "grid")
                    {
                        firstObjectPos = gridManager.GetGridPosition(worldPositionVector2);
                    }
                    else firstObjectClicked = null;
                }
                else if (secondObjectClicked == null && firstObjectClicked != null && secondObjectClicked != firstObjectClicked)
                {
                    secondObjectClicked = hit.collider.gameObject;

                if (secondObjectClicked != firstObjectClicked)
                {
                    secondObjectPos = gridManager.GetGridPosition(worldPositionVector2);
                    ClickInteractions();
                }
                else secondObjectClicked = null;
                  
                    
                }
                else if (secondObjectClicked != null && firstObjectClicked != null)
                {
                    firstObjectClicked = hit.collider.gameObject;
                    firstObjectPos = gridManager.GetGridPosition(worldPositionVector2);
                    secondObjectClicked = null;
                }
            }
            else if (hit.collider == null)
            {
                firstObjectClicked = null;
                secondObjectClicked = null;
            }
        
    }

    public void ClickInteractions()
    {
        if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "grass" && gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "none"
            )
        {
            if (gridManager.grid[secondObjectPos.x, secondObjectPos.y].isOccupied)
            {
                ClearAllClickedObjects();
                return;
            }
            //if (secondObjectClicked != null && secondObjectClicked != firstObjectClicked)
            //{
            //    objectToMove = firstObjectClicked;
            //    MoveObjectToGrid(secondObjectPos);
            //    ClearAllClickedObjects();
            //}


            if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "rabbit")
            {
                if (gridManager.grid[firstObjectPos.x, firstObjectPos.y].action > 0)
                {
                    if ( AnimalMoveDestinations("rabbit", firstObjectPos).Contains(secondObjectPos))
                    {
                        objectToMove = firstObjectClicked;
                        MoveObjectToGrid(secondObjectPos);
                        ClearAllClickedObjects();
                    }
                    else ClearAllClickedObjects();

                    //if (secondObjectPos.x == firstObjectPos.x + 1 || secondObjectPos.x == firstObjectPos.x - 1)
                    //{
                    //    if (secondObjectPos.y == firstObjectPos.y)
                    //    {
                    //        objectToMove = firstObjectClicked;
                    //        MoveObjectToGrid(secondObjectPos);
                    //        ClearAllClickedObjects();
                    //    }
                    //    else ClearAllClickedObjects();
                    //}
                    //else if (secondObjectPos.y == firstObjectPos.y + 1 || secondObjectPos.y == firstObjectPos.y - 1)
                    //{
                    //    if (secondObjectPos.x == firstObjectPos.x)
                    //    {
                    //        objectToMove = firstObjectClicked;
                    //        MoveObjectToGrid(secondObjectPos);
                    //        ClearAllClickedObjects();
                    //    }
                    //    else ClearAllClickedObjects();
                    //}
                    //else ClearAllClickedObjects();
                }
                else ClearAllClickedObjects();

            }
        }
    }

    void MoveObjectToGrid(Vector2Int gridPos)
    {
        if (objectToMove != null)
        {
            gridManager.SetCell(gridPos.x, gridPos.y, gridManager.grid[firstObjectPos.x, firstObjectPos.y].content, 
             gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType);
            gridManager.RemoveContent(firstObjectPos.x, firstObjectPos.y);
            gridManager.grid[gridPos.x, gridPos.y].action -= 1;
            Destroy(firstObjectClicked);
            objectToMove = null;

        }
    }

    void ClearAllClickedObjects()
    {
        firstObjectClicked = null;
        secondObjectClicked = null;
    }

    public Vector2Int[] AnimalMoveDestinations( string animalType, Vector2Int animalPoisition)
    {
        if (animalType == "rabbit")
        {
            Vector2Int[] animalMoveDestinations = new Vector2Int[4];
            animalMoveDestinations[0] = new Vector2Int(animalPoisition.x - 1, animalPoisition.y);
            animalMoveDestinations[1] = new Vector2Int(animalPoisition.x + 1, animalPoisition.y);
            animalMoveDestinations[2] = new Vector2Int(animalPoisition.x , animalPoisition.y + 1);
            animalMoveDestinations[3] = new Vector2Int(animalPoisition.x , animalPoisition.y - 1);
            return animalMoveDestinations;
        }
        else return new Vector2Int[0];
    }
    public void SelectionGridHighlight()
    {
        if (firstObjectClicked != null && gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "grass"
            && gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType != "none")
        {
            foreach (Vector2Int highlightGrid in AnimalMoveDestinations(gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType, firstObjectPos))
            {
                gridManager.SetCellHighlighted(highlightGrid);
            }
        }

        else if (firstObjectClicked == null || gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "grass"
            || gridManager.grid[firstObjectPos.x, firstObjectPos.y].objectType == "none")
        {
            gridManager.ClearAllHightlights();
        }
    }
}
