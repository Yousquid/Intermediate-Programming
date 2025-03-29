using UnityEditor.SearchService;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;

    public GameObject backgroundPrefab;  // Prefab for the background tile sprite
    public float cellSize = 1.0f;        // Size of each grid cell (adjust if needed)
    public Vector3 gridOrigin; // Starting position of the grid


    public GridCell[,] grid;
    public GameObject[,] gridObject;

    public GameObject grass;

    public GameObject rabbit;
    public GameObject wolf;

    public GameObject highlight;
    public GameObject mateSign;
    public GameObject hole;

    public UIManager UIManager;

    private bool hasMateSignSpawned = false;

    void Start()
    {
        // Initialize the grid
        grid = new GridCell[gridWidth, gridHeight];
        InitializeGrid();
       // this.transform.position += gridOrigin;

    }

    // Initialize the grid with background tiles

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCell(Random.Range(0, gridWidth), Random.Range(0, gridHeight), grass,  "grass","",false);
           
            SetCell(2, 3, grass,  "grass","",false);
            SetCell(Random.Range(0, gridWidth), Random.Range(0, gridHeight), wolf,  "wolf","",false);

            
        }
        GridImageUpdate();
    }
    void InitializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new GridCell(null, false, "", 0, "", false);
                SetCell(x, y, null, "", "", false);
                // 计算本地坐标后加上 gridOrigin
                Vector3 localPosition = new Vector3(x * cellSize, y * cellSize, 0);
                CreateBackgroundTile(localPosition);
            }
        }

    }

    void GridImageUpdate()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].content == grass && !grid[x, y].isOccupied)
                {
                    // 使用 gridOrigin 计算世界坐标
                    Vector3 spawnPos = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                    GameObject Grass = Instantiate(grass, spawnPos, Quaternion.identity);
                    Grass.transform.parent = transform;
                    grid[x, y].isOccupied = true;
                }
                else if (grid[x, y].content == rabbit && !grid[x, y].isOccupied)
                {
                    Vector3 spawnPos = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                    GameObject Rabbit = Instantiate(rabbit, spawnPos, Quaternion.identity);
                    Rabbit.transform.parent = transform;
                    grid[x, y].isOccupied = true;
                }
                else if (grid[x, y].content == wolf && !grid[x, y].isOccupied)
                {
                    Vector3 spawnPos = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                    GameObject Rabbit = Instantiate(wolf, spawnPos, Quaternion.identity);
                    Rabbit.transform.parent = transform;
                    grid[x, y].isOccupied = true;
                }

                if (grid[x, y].backgroundType == "hole" && !grid[x, y].hasBackground)
                {
                    Vector3 spawnPos = gridOrigin + new Vector3(x * cellSize, y * cellSize, 0);
                    GameObject Hole = Instantiate(hole, spawnPos, Quaternion.identity);
                    Hole.transform.parent = transform;
                    grid[x, y].hasBackground = true;
                }
            }
        }
    }



    // Create a background tile at the given position
    void CreateBackgroundTile(Vector3 position)
    {
        Vector3 worldPosition = gridOrigin + position;
        GameObject backgroundTile = Instantiate(backgroundPrefab, worldPosition, Quaternion.identity);
        backgroundTile.transform.parent = transform;
        backgroundTile.name = "BackgroundTile_" + worldPosition.ToString();
    }

    // Set the content of a specific grid cell
    public void SetCell(int x, int y, GameObject content, string objectType, string backgrounType, bool hasBackground)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            grid[x, y] = new GridCell(content, false, objectType, 1, backgrounType,hasBackground);
        }
    }

    // Remove content from a specific grid cell
    public void RemoveContent(int x, int y, string backgrounType,bool hasBackground)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            SetCell(x, y, null, "", backgrounType,hasBackground);  // Reset to empty cell
           // GameObject backgroundTile = Instantiate(backgroundPrefab, new Vector3(x * cellSize-4, y * cellSize-4, 0), Quaternion.identity);
          //  backgroundTile.transform.parent = transform;
        }
    }

    // Check if a cell is occupied
    public bool IsCellOccupied(int x, int y)
    {
        return grid[x, y].isOccupied;
    }

    public void SetCellHighlighted(Vector2Int highlightPos)
    {
        if (grid[highlightPos.x, highlightPos.y].content != highlight && !grid[highlightPos.x, highlightPos.y].isOccupied)
        {
            // 使用 gridOrigin 计算坐标
            Vector3 spawnPos = gridOrigin + new Vector3(
                highlightPos.x * cellSize,
                highlightPos.y * cellSize,
                0
            );

            GameObject Hightlight = Instantiate(highlight, spawnPos, Quaternion.identity);
            grid[highlightPos.x, highlightPos.y].content = highlight;
            Hightlight.transform.parent = transform;
        }

    }
    public void SetCellMateSign(Vector2Int highlightPos)
    {
        if (!hasMateSignSpawned)
        {
            // 使用 gridOrigin 计算坐标
            Vector3 spawnPos = gridOrigin + new Vector3(
                highlightPos.x * cellSize,
                highlightPos.y * cellSize,
                0
            );

            GameObject MateSign = Instantiate(mateSign, spawnPos, Quaternion.identity);
            MateSign.transform.parent = transform;
            hasMateSignSpawned = true;
        }
    }

    public void ClearAllHightlights()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].content == highlight)
                {
                    string backgroundType = grid[x, y].backgroundType;
                    bool hasBackground = grid[x, y].hasBackground;
                    RemoveContent(x, y,backgroundType,hasBackground);
                }
            }
        }
        GameObject[] objects = GameObject.FindGameObjectsWithTag("highlight");
        foreach (GameObject gamobject in objects)
        { 
            Destroy(gamobject);
        }
        GameObject[] mateObjects = GameObject.FindGameObjectsWithTag("matesign");
        foreach (GameObject gamobject in mateObjects)
        {
            Destroy(gamobject);
        }
        hasMateSignSpawned = false;
    }
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.y - gridOrigin.y) / cellSize);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridOrigin.x + gridPos.x * cellSize, gridOrigin.y + gridPos.y * cellSize, 0);
    }

    public void EndTurn()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].objectType == "grass")
                {
                    grid[x, y].content.GetComponent<GridContentTrigger>().leftResource -=
                        CountSorroundingGridsOfObjectType(grid, x, y, "rabbit")*2;
                    UIManager.food += CountSorroundingGridsOfObjectType(grid, x, y, "rabbit")*2;
                }
                
                if (grid[x, y].objectType == "rabbit")
                {
                    UIManager.food += -1;
                }
            }
        }

        
    }

    public int CountSorroundingGridsOfObjectType(GridCell [,] grid, int x, int y, string objectType)
    {
        int count = 0;
        for (int w = -1; w < 2; w++)
        {
            for (int h = -1; h < 2; h++)
            {
                int neighborX = x + w;
                int neighborY = y + h;
                if (w == 0 && h == 0)
                {
                }
                else if (neighborX < 0 || neighborY < 0 || neighborX >= grid.GetLength(0) || neighborY >= grid.GetLength(1))
                {

                }
                else if (grid[neighborX, neighborY].objectType == objectType)
                {
                    count += 1;
                }
            }
        }
        return count;
    }

}