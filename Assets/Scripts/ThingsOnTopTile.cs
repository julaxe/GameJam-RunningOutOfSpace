using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingsOnTopTile : MonoBehaviour
{
    private Tile _tile;

    private void Awake()
    {
        _tile = transform.parent.GetComponent<Tile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _tile.AddObjectOnTop(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _tile.DeleteObjectOnTop(other.gameObject);
    }
}
