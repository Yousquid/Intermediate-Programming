using UnityEngine;

public class TrunManager : MonoBehaviour
{
    public bool isSelected = false;
    public GridManager gridManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        gridManager.EndTurn();

        StartTurn();
    }

    void StartTurn()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                gridManager.grid[x, y].action = 1;

            }
        }
    }
}
