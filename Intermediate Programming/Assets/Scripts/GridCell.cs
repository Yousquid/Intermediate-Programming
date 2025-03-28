using UnityEngine;
public class GridCell
{
    public GameObject content; // The object or thing occupying the grid cell (could be a character, item, etc.)
    public bool isOccupied;    // Whether the cell is currently occupied
    public string objectType;  // The type of object (optional, if you want to differentiate things like enemies, obstacles, etc.)
    public Sprite gridBackground;
    public int action;
    public string backgroundType;
    public bool hasBackground;
    public GridCell(GameObject content, bool isOccupied, string objectType, int action, string backgroundType, bool hasBackground)
    {
        this.content = content;
        this.isOccupied = isOccupied;
        this.objectType = objectType;
        this.action = action;
        this.backgroundType = backgroundType;
        this.hasBackground = hasBackground;
    }

}