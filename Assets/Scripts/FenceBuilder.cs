using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FenceBuilder : MonoBehaviour
{
    public bool isOn;
    private GameObject _currentTile;

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
        }
    }
    void CheckIfCanBuildFenceInTile()
    {
        if (Camera.main is { })
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit))
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
}
