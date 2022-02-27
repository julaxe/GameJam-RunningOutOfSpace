using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{

    private List<GameObject> _neighbors;
    public bool hasFenceOnTop;
    public bool isCounted;

    private BoxCollider _fenceBoxCollider;
    private NavMeshObstacle _fenceNavMeshObstacle;
    private GameObject _fence;
    private Material _greenMaterial;
    private Material _redMaterial;
    private Material _blackMaterial;
    
    void Awake()
    {
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
    }



}
