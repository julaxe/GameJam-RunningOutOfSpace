using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    public int gridSize = 20;
    public GameObject tilePrefab;
    public Transform topLeftCorner;
    public GameObject[,] Grid;
    private GameScaleManager _scaleManager;
    private NavMeshSurface _surface;
    
    void Start()
    {
        Grid = new GameObject[gridSize, gridSize];
        _scaleManager = FindObjectOfType<GameScaleManager>();
        _surface = GetComponent<NavMeshSurface>();
        CreateGrid();
        SetupNeighbors();
        BuildFenceAround();
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
                    Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i-1, j]);
                    //top
                    if (j - 1 >= 0)
                    {
                        Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i-1, j-1]);
                    }
                    //bottom
                    if (j + 1 < gridSize)
                    {
                        Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i-1, j+1]);
                    }
                }
                //center - top
                if (j - 1 >= 0)
                {
                    Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i, j-1]);
                }
                //center - bottom
                if (j + 1 < gridSize)
                {
                    Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i, j+1]);
                }
                //right side
                if (i + 1 < gridSize)
                {
                    Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i+1, j]);
                    //top
                    if (j - 1 >= 0)
                    {
                        Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i+1, j-1]);
                    }
                    //bottom
                    if (j + 1 < gridSize)
                    {
                        Grid[i, j].GetComponent<Tile>().AddNewNeighbor(Grid[i+1, j+1]);
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
                newTile.GetComponent<Tile>().column = i;
                newTile.GetComponent<Tile>().row = j;
                Grid[i, j] = newTile;
            }
        }
    }

    void BuildFenceAround()
    {
        for (int i = 0; i < gridSize; i++)
        {
            Grid[i,0].GetComponent<Tile>().BuildFence();
            Grid[0,i].GetComponent<Tile>().BuildFence();
            Grid[gridSize - 1,i].GetComponent<Tile>().BuildFence();
            Grid[i,gridSize - 1].GetComponent<Tile>().BuildFence();
            
        }
    }

}
