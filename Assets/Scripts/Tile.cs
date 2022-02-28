using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{

    private List<GameObject> _neighbors;
    private List<GameObject> _neighborsWithFence;
    public bool hasFenceOnTop;
    public bool isCounted;
    public int column;
    public int row;

    private BoxCollider _fenceBoxCollider;
    private NavMeshObstacle _fenceNavMeshObstacle;
    private GameObject _fence;
    private Material _greenMaterial;
    private Material _redMaterial;
    private Material _blackMaterial;
    private List<GameObject> _objectsOnTop;

    void Awake()
    {
        _objectsOnTop = new List<GameObject>();
        _neighbors = new List<GameObject>();
        _fence = transform.Find("Fence").gameObject;
        _fenceBoxCollider = _fence.GetComponent<BoxCollider>();
        _fenceNavMeshObstacle = _fence.GetComponent<NavMeshObstacle>();
        LoadMaterials();
        _fenceBoxCollider.enabled = false;
        _fenceNavMeshObstacle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LoadMaterials()
    {
        _greenMaterial = Resources.Load<Material>("Materials/GreenMaterial");
        _redMaterial = Resources.Load<Material>("Materials/RedMaterial");
        _blackMaterial = Resources.Load<Material>("Materials/BlackMaterial");
    }

    public void AddNewNeighbor(GameObject newTile)
    {
        _neighbors.Add(newTile);
    }

    public List<GameObject> GetNeighborsWithFence()
    {
        UpdateNeighborsWithFence();
        return _neighborsWithFence;
    }

    public List<GameObject> GetNeighbors()
    {
        return _neighbors;
    }

    private void UpdateNeighborsWithFence()
    {
        _neighborsWithFence = new List<GameObject>();
        foreach (var neighbor in _neighbors)
        {
            if (neighbor.GetComponent<Tile>().hasFenceOnTop)
                _neighborsWithFence.Add(neighbor);
        }
    }

public void ShowFence()
    {
        _fence.SetActive(true);
    }

    public void HideFence()
    {
        _fence.SetActive(false);
    }

    public void BuildFence()
    {
        //change material to black.
        _fence.GetComponent<MeshRenderer>().material = _blackMaterial;
        //rebuild navmesh
        _fenceBoxCollider.enabled = true;
        _fenceNavMeshObstacle.enabled = true;
        
        hasFenceOnTop = true;
        if (!_fence.activeInHierarchy) _fence.SetActive(true);
    }

    public List<GameObject> GetOppositeBlankTiles()
    {
        var newList = new List<GameObject>();
        foreach (var neighbor1 in _neighbors)
        {
            foreach (var neighbor2 in _neighbors.Where(neighbor2 => neighbor1 != neighbor2))
            {
                if (neighbor1.GetComponent<Tile>().column == neighbor2.GetComponent<Tile>().column - 2 || neighbor1.GetComponent<Tile>().column == neighbor2.GetComponent<Tile>().column + 2)
                {
                    if (!neighbor1.GetComponent<Tile>().hasFenceOnTop && !neighbor2.GetComponent<Tile>().hasFenceOnTop)
                    {
                        newList.Add(neighbor1);
                        newList.Add(neighbor2);
                        return newList;
                    }
                }
                if (neighbor1.GetComponent<Tile>().row == neighbor2.GetComponent<Tile>().row - 2 || neighbor1.GetComponent<Tile>().row == neighbor2.GetComponent<Tile>().row + 2)
                {
                    if (!neighbor1.GetComponent<Tile>().hasFenceOnTop && !neighbor2.GetComponent<Tile>().hasFenceOnTop)
                    {
                        newList.Add(neighbor1);
                        newList.Add(neighbor2);
                        return newList;
                    }
                }
            }
        }
        return null;
    }

    public void AddObjectOnTop(GameObject newObj)
    {
        _objectsOnTop.Add(newObj);
    }

    public void DeleteObjectOnTop(GameObject objToDelete)
    {
        _objectsOnTop.Remove(objToDelete);
    }

    public List<GameObject> GetThingsOnTop()
    {
        return _objectsOnTop;
    }

}
