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
    public GameObject winScreen;
    public GameObject losingScreen;
    
    private GameObject _currentTile;
    private MapManager _mapManager;
    private int _tileCounter;

    private List<GameObject> _listOfTiles1;
    private List<GameObject> _listOfTiles2;
    
    //sound
    private AudioSource _audioSource;
    private AudioClip _winClip;
    private AudioClip _buildClip;
    
    

    private void Awake()
    {
        Time.timeScale = 1.0f;
        _listOfTiles1 = new List<GameObject>();
        _listOfTiles2 = new List<GameObject>();
        _mapManager = FindObjectOfType<MapManager>();
        _audioSource = GetComponent<AudioSource>();
        _winClip = Resources.Load<AudioClip>("Sounds/win");
        _buildClip = Resources.Load<AudioClip>("Sounds/build");
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
            _audioSource.PlayOneShot(_buildClip);
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
        CountNeighbors1(oppositeTiles[0]);
        var side1 = _tileCounter;
        ClearCounter();
        CountNeighbors2(oppositeTiles[1]);
        var side2 = _tileCounter;
        
        //check always the smaller side
        if (side1 < side2)
        {
            var tilesNeeded = CountTilesNeeded(_listOfTiles1);
            if (tilesNeeded > _listOfTiles1.Count)
            {
                //good, continues playing
                _audioSource.PlayOneShot(_winClip);
            }
            else
            {
                if (tilesNeeded != 0)
                {
                    //bad, game over.
                    Time.timeScale = 0.0f;
                    losingScreen.SetActive(true); 
                }
            }
            Debug.Log(tilesNeeded);
        }
        else
        {
            var tilesNeeded = CountTilesNeeded(_listOfTiles2);
            if (tilesNeeded > _listOfTiles2.Count)
            {
                //good, continues playing
                _audioSource.PlayOneShot(_winClip);
            }
            else
            {
                if (tilesNeeded != 0)
                {
                    //bad, game over.
                    Time.timeScale = 0.0f;
                    losingScreen.SetActive(true); 
                }
            }
            Debug.Log(tilesNeeded);
        }

    }

    private int CountTilesNeeded(List<GameObject> listOfTiles)
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach (var tile in listOfTiles)
        {
            var thingsOnTopOfTile = tile.GetComponent<Tile>().GetThingsOnTop();
            foreach (var thing in thingsOnTopOfTile)
            {
                if (thing.CompareTag("Enemy"))
                {
                    if(!enemies.Contains(thing))
                        enemies.Add(thing);
                }
            }
        }

        int tilesNeeded = 0;
        foreach (var enemy in enemies)
        {
            tilesNeeded += enemy.GetComponent<EnemyAI>().tiles;
        }

        return tilesNeeded;
    }

    private void CountNeighbors1(GameObject currentTile)
    {
        currentTile.GetComponent<Tile>().isCounted = true;
        _listOfTiles1.Add(currentTile);
        _tileCounter += 1;
        foreach (var neighbor in currentTile.GetComponent<Tile>().GetNeighbors())
        {
            if (!neighbor.GetComponent<Tile>().hasFenceOnTop && !neighbor.GetComponent<Tile>().isCounted)
            {
                CountNeighbors1(neighbor);
            }
        }
    }
    private void CountNeighbors2(GameObject currentTile)
    {
        currentTile.GetComponent<Tile>().isCounted = true;
        _listOfTiles2.Add(currentTile);
        _tileCounter += 1;
        foreach (var neighbor in currentTile.GetComponent<Tile>().GetNeighbors())
        {
            if (!neighbor.GetComponent<Tile>().hasFenceOnTop && !neighbor.GetComponent<Tile>().isCounted)
            {
                CountNeighbors2(neighbor);
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
