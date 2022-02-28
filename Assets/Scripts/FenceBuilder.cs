using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class FenceBuilder : MonoBehaviour
{
    public bool isOn;
    public LayerMask whatIsGround;
    private GameObject _currentTile;
    private MapManager _mapManager;
    private int _tileCounter;

    private void Awake()
    {
        _mapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        if (isOn)
        {
            CheckIfCanBuildFenceInTile();
        }
    }

    public void OnBuild(InputValue value)
    {
        if (_currentTile)
        {
            if (_currentTile.GetComponent<Tile>().hasFenceOnTop) return;
            _currentTile.GetComponent<Tile>().BuildFence();
            IsPenClosed();
        }
    }
    void CheckIfCanBuildFenceInTile()
    {
        if (Camera.main is { })
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, whatIsGround))
            {
                if (!hit.transform.gameObject.GetComponent<Tile>()) return;
                //if its a tile
                //check if current tile changed
                if (_currentTile && _currentTile != hit.transform.gameObject)
                {
                    //clear current tile
                    if (!_currentTile.GetComponent<Tile>().hasFenceOnTop)
                        _currentTile.GetComponent<Tile>().HideFence();
                }
                //show fence
                _currentTile = hit.transform.gameObject;
                if (!_currentTile.GetComponent<Tile>().hasFenceOnTop)
                    _currentTile.GetComponent<Tile>().ShowFence();
            }
        }
    }
    bool IsPenClosed()
    {
        List<GameObject> path = new List<GameObject>();
        IsPathDoable(path, _currentTile, 0);

        return false;
    }

    private bool IsPathDoable(List<GameObject> path, GameObject currentTile, int numberOfTilesInPath)
    {
        var neighborsWithFence = currentTile.GetComponent<Tile>().GetNeighborsWithFence();
        if (neighborsWithFence.Count < 2) return false;
        if (numberOfTilesInPath > 35) return false;
        //for each neighbor create a new path if is not the _currentTile.
        foreach (var neighbor in neighborsWithFence)
        {
            if(path.Count > 1)
                if (path[path.Count - 1] == neighbor) continue;
            
            if (neighbor == _currentTile && numberOfTilesInPath > 4)
            {
                CountBlankTilesInside(path);
                return true;
            }
            if (path.Contains(neighbor))
            {
                continue;
            }

            var newPath = new List<GameObject>(path);
            newPath.Add(currentTile);
            if(IsPathDoable(newPath, neighbor, numberOfTilesInPath + 1))
            {
                return true;
            }
        }

        return false;
    }

    private void CountBlankTilesInside(List<GameObject> tiles)
    {
        //the current tile
        var closingTile = tiles[0];
        //for now we are not gonna think in more shapes than rectangles
        var oppositeTiles = closingTile.GetComponent<Tile>().GetOppositeBlankTiles();
        if (oppositeTiles == null)
        {
            Debug.Log("couldn't get opposite tiles");
            return;
        }
        ClearCounter();
        CountNeighbors(oppositeTiles[0]);
        Debug.Log("side 1: " + _tileCounter);
        ClearCounter();
        CountNeighbors(oppositeTiles[1]);
        Debug.Log("side 2: " +_tileCounter);

    }

    private void CountNeighbors(GameObject currentTile)
    {
        currentTile.GetComponent<Tile>().isCounted = true;
        _tileCounter += 1;
        foreach (var neighbor in currentTile.GetComponent<Tile>().GetNeighbors())
        {
            if (!neighbor.GetComponent<Tile>().hasFenceOnTop && !neighbor.GetComponent<Tile>().isCounted)
            {
                CountNeighbors(neighbor);
            }
        }
    }

    private void ClearCounter()
    {
        _tileCounter = 0;
        foreach (var tile in _mapManager.Grid)
        {
            tile.GetComponent<Tile>().isCounted = false;
        }
        
    }
    
    
}
