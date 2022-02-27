using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    public int gridSize = 20;
    public GameObject tilePrefab;
    public Transform topLeftCorner;
    private GameObject[,] _grid;
    private GameScaleManager _scaleManager;
    private NavMeshSurface _surface;
    
    void Start()
    {
        _grid = new GameObject[gridSize, gridSize];
        _scaleManager = FindObjectOfType<GameScaleManager>();
        _surface = GetComponent<NavMeshSurface>();
        CreateGrid();
        SetupNeighbors();
        _scaleManager.SetScale(0.6f);
        _surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SetupNeighbors()
    {
        
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                //left side
                if (i - 1 >= 0)
                {
                    _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i-1, j]);
                    //top
                    if (j - 1 >= 0)
                    {
                        _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i-1, j-1]);
                    }
                    //bottom
                    if (j + 1 < gridSize)
                    {
                        _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i-1, j+1]);
                    }
                }
                //center - top
                if (j - 1 >= 0)
                {
                    _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i, j-1]);
                }
                //center - bottom
                if (j + 1 < gridSize)
                {
                    _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i, j+1]);
                }
                //right side
                if (i + 1 < gridSize)
                {
                    _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i+1, j]);
                    //top
                    if (j - 1 >= 0)
                    {
                        _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i+1, j-1]);
                    }
                    //bottom
                    if (j + 1 < gridSize)
                    {
                        _grid[i, j].GetComponent<Tile>().AddNewNeighbor(_grid[i+1, j+1]);
                    }
                }
                
            }
        }
    }
    void CreateGrid()
    {
        float initialPosX = topLeftCorner.position.x;
        float initialPosZ = topLeftCorner.position.z;
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform);
                newTile.transform.position = new Vector3(initialPosX - i, 0.0f, initialPosZ + j);
                _grid[i, j] = newTile;
            }
        }
    }
}
