using UnityEngine;
using System.Collections.Generic; 

public class BuildingManager : MonoBehaviour
{
    GridManager gridManager;
    ClickEventer clickEventer;
    public List<Vector2Int> holePoses = new List<Vector2Int>();
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        clickEventer = GetComponent<ClickEventer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DigHole()
    {
        if (gridManager.grid[clickEventer.firstObjectPos.x, clickEventer.firstObjectPos.y].action > 0)
        {
            gridManager.grid[clickEventer.firstObjectPos.x, clickEventer.firstObjectPos.y].backgroundType = "hole";
            holePoses.Add(new Vector2Int(clickEventer.firstObjectPos.x, clickEventer.firstObjectPos.y));
            gridManager.grid[clickEventer.firstObjectPos.x, clickEventer.firstObjectPos.y].action--;
        }
       
    }

    public Vector2Int GiveAnotherHolePosition(Vector2Int thisHolePos)
    {
        if (holePoses.Contains(thisHolePos))
        {
            if (holePoses.IndexOf(thisHolePos) % 2 == 0 && holePoses[holePoses.IndexOf(thisHolePos) + 1] != null)
            {
                return holePoses[holePoses.IndexOf(thisHolePos) + 1];
            }
            else if (holePoses.IndexOf(thisHolePos) % 2 == 0 && holePoses[holePoses.IndexOf(thisHolePos) + 1] == null)
            { 
                return Vector2Int.zero;
            }
            else
            {
                return holePoses[holePoses.IndexOf(thisHolePos) - 1];
            }
        }
        else return Vector2Int.zero;
    }
}
