using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;

    public GameObject backgroundPrefab;  // Prefab for the background tile sprite
    public float cellSize = 1.0f;        // Size of each grid cell (adjust if needed)
    public Vector3 gridOrigin; // Starting position of the grid


    public GridCell[,] grid;

    public GameObject grass;
    public GameObject rabbit;

    void Start()
    {
        // Initialize the grid
        grid = new GridCell[gridWidth, gridHeight];
        InitializeGrid();
        this.transform.position += new Vector3(-4, -4, 0);
    }

    // Initialize the grid with background tiles

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCell(Random.Range(0, gridWidth), Random.Range(0, gridHeight), grass,  "grass");
           
            SetCell(2, 3, grass,  "grass");
            SetCell(Random.Range(0, gridWidth), Random.Range(0, gridHeight), rabbit,  "rabbit");
            
        }
        GridImageUpdate();
    }

    private void LateUpdate()
    {
       
    }
    void InitializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new GridCell();
                SetCell(x, y, null, "none");
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0); // Calculate position of the background sprite
                CreateBackgroundTile(position);
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
                    //"-4" means the offset of the camera center, same with the value at the start
                    GameObject Grass = Instantiate(grass, new Vector3(x * cellSize -4, y * cellSize-4, 0), Quaternion.identity);
                    Grass.transform.parent = transform;
                    grid[x, y].isOccupied = true;
                }
                //else if (grid[x, y].content == null)
                //{
                //    GameObject backgroundTile = Instantiate(backgroundPrefab, new Vector3(x * cellSize, y * cellSize, 0), Quaternion.identity);
                //    backgroundTile.transform.parent = transform;
                //}
                else if (grid[x, y].content == rabbit && !grid[x, y].isOccupied)
                {
                    GameObject Rabbit = Instantiate(rabbit, new Vector3(x * cellSize-4, y * cellSize-4, 0), Quaternion.identity);
                    Rabbit.transform.parent = transform;
                    grid[x, y].isOccupied = true;
                }

            }
        }
    }



    // Create a background tile at the given position
    void CreateBackgroundTile(Vector3 position)
    {
        GameObject backgroundTile = Instantiate(backgroundPrefab, position, Quaternion.identity);
        backgroundTile.transform.parent = transform;  // Make it a child of the GridManager for easier management
        backgroundTile.name = "BackgroundTile_" + position.ToString();
    }

    // Set the content of a specific grid cell
    public void SetCell(int x, int y, GameObject content, string objectType = "")
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            grid[x, y] = new GridCell(content, false, objectType);
        }
    }

    // Remove content from a specific grid cell
    public void RemoveContent(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            SetCell(x, y, null, "none");  // Reset to empty cell
            GameObject backgroundTile = Instantiate(backgroundPrefab, new Vector3(x * cellSize-4, y * cellSize-4, 0), Quaternion.identity);
            backgroundTile.transform.parent = transform;
        }
    }

    // Check if a cell is occupied
    public bool IsCellOccupied(int x, int y)
    {
        return grid[x, y].isOccupied;
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
}