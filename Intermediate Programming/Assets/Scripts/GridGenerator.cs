using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GridGenerator : MonoBehaviour
{
    public  float TILE_SIZE = 2f;

    public GameObject wallPrefab;

    // These are randomized by the task randomizer
    public int gridWidth = 20;
    public int gridHeight = 10;

    bool[,] _wallGrid;
    bool[,] _nextWallGrid;

    public float gridDensity;
    public int overflowMaxNeighborsNumber;
    public int lonelyMinimumNeighborsNumber;
    public int groupMinimumNeighborsNumber;

    void Start()
    {
        _wallGrid = new bool[gridWidth, gridHeight];
        _nextWallGrid = new bool[gridWidth, gridHeight];

        FillGrid();
        SpawnWalls();
        //SpawnPlayer();

        // Move ourself based on our width
        float offsetX = gridWidth * TILE_SIZE / 2f - TILE_SIZE / 2f;
        float offsetY = gridHeight * TILE_SIZE / 2f - TILE_SIZE / 2f;
        transform.position -= new Vector3(offsetX, offsetY, 0);
    }

    void FillGrid()
    {
        // Your code for 2-a here:

        for (int w = 0; w < gridWidth; w++)
        {
            for (int h = 0; h < gridHeight; h++)
            {
                _wallGrid[w, h] = true;
            }

        }
        // Feel free to fill the grid with random walls. 
        // Keep in mind that this will be the starting point for the CA steps.

    }
    Vector2Int GetPlayerSpawnPos()
    {
        // Your code for 2-a here:
        int xSpwanCoordinate;
        int ySpwanCoordinate;

        do
        {
            xSpwanCoordinate = Random.Range(1, gridWidth);
            ySpwanCoordinate = Random.Range(1, gridHeight);
        }
        while (_wallGrid[xSpwanCoordinate, ySpwanCoordinate] == true);

        if (_wallGrid[xSpwanCoordinate, ySpwanCoordinate] != true)
        {
            return new Vector2Int(xSpwanCoordinate, ySpwanCoordinate);
        }
        else return Vector2Int.zero;
        // Default implementation: You'll want to replace this
    }

    bool NextCAValue(int x, int y)
    {
        // Your code for 2-b here:
        bool[,] newGridMap = new bool[gridWidth, gridHeight];


        if (CountSorroundingLiveWalls(_wallGrid, x, y) > overflowMaxNeighborsNumber)
        {
            newGridMap[x, y] = false;
        }
        if (CountSorroundingLiveWalls(_wallGrid, x, y) < lonelyMinimumNeighborsNumber)
        {
            newGridMap[x, y] = false;
        }
        if (CountSorroundingLiveWalls(_wallGrid, x, y) > groupMinimumNeighborsNumber)
        {
            newGridMap[x, y] = true;
        }
        return newGridMap[x, y];



        // A common way to approach CA for dungeon/cave generation is to 
        // count the number of wall neighbors to a given point and decide
        // based on that number if a wall should appear or remain in the given point.
        // Note that _wallGrid contains the current wall grid and shouldn't be modified until the end of the CA step.

        // Default implementation: you'll want to replace this
        return Random.value <= 0.5f;
    }

    public int CountSorroundingLiveWalls(bool[,] map, int x, int y)
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
                else if (neighborX < 0 || neighborY < 0 || neighborX >= map.GetLength(0) || neighborY >= map.GetLength(1))
                {

                }
                else if (map[neighborX, neighborY])
                {
                    count += 1;
                }
            }
        }
        return count;
    }

    void PerformCAStep()
    {
        // How a CA step is performed.

        // First, we create the next grid by using nextCAValue for each grid location.
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                _nextWallGrid[x, y] = NextCAValue(x, y);
            }
        }

        // Then we update the current grid to match _nextWallGrid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                _wallGrid[x, y] = _nextWallGrid[x, y];
            }
        }
    }



    void RespawnWalls()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        SpawnWalls();
    }

    void SpawnWalls()
    {
        // Spawns the walls according to _wallGrid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (_wallGrid[x, y])
                {
                    GameObject wallObj = Instantiate(wallPrefab);
                    wallObj.transform.parent = transform;
                    wallObj.transform.localPosition = new Vector3(x * TILE_SIZE, y * TILE_SIZE, 0);
                }
            }
        }
    }

 




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformCAStep();
            RespawnWalls();
        }
    }
}
