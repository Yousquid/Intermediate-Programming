using UnityEngine;

public class GridContentTrigger : MonoBehaviour
{
    public bool isSelected;
    public string objectType;
    public GridManager gridManager;
    public int leftResource;
    public Vector2Int thisPos;
    void Start()
    {
        gridManager = gameObject.GetComponent<GridManager>();

        if (objectType == "grass")
        {
            leftResource = 50;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 worldPositionVector2 = Camera.main.ScreenToWorldPoint(this.transform.position);
        //thisPos = gridManager.GetGridPosition(worldPositionVector2);
    }

    void EndTurn()
    {
        if (objectType == "grass")
        {
            for (int i = -1; i < 2; i++)
            {
                if (gridManager.grid[thisPos.x + i, thisPos.y + i].objectType == "rabbit")
                {
                    leftResource -= 1;
                }
                if (gridManager.grid[thisPos.x, thisPos.y + i].objectType == "rabbit")
                {
                    leftResource -= 1;
                }
            }
        }
    }
}
