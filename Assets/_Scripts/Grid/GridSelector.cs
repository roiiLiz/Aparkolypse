using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [Header("Grid Selection")]
    [SerializeField]
    private GameObject selectionPreview;
    [SerializeField]
    private Tower selectedTowerPrefab;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GridInput input;
    [Header("Sphere Overlap")]
    [SerializeField]
    private float sphereRadius = 1f;
    [SerializeField]
    private float maxGridLength = 11f;

    private MeshRenderer selectionMesh;

    public static event Action<GameObject> OnPlaceCart;

    private void OnEnable () { UnitShop.OnSelectNewTower += UpdateSelection; }
    private void OnDisable () { UnitShop.OnSelectNewTower -= UpdateSelection; }

    private void Start()
    {
        selectionMesh = selectionPreview.GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    private void Update()
    {
        Vector3 selectedPosition = input.GetGridPosition();

        Vector3Int gridCellPosition = grid.WorldToCell(selectedPosition);

        selectionPreview.transform.position = grid.GetCellCenterWorld(gridCellPosition) - new Vector3(0, 0.5f, 0);

        selectionMesh.enabled = input.allowPlacement;

        if (input.GetGridInput() && GridTileIsEmpty())
        {
            // CheckAdjacentTiles();
            var cart = Instantiate(selectedTowerPrefab.prefab, selectionPreview.transform.position, Quaternion.identity, grid.gameObject.transform);
            OnPlaceCart?.Invoke(cart);
        }
    }

    public void UpdateSelection(Tower incomingSelection)
    {
        selectedTowerPrefab = incomingSelection;
    }

    private bool GridTileIsEmpty()
    {
        Collider[] interstectingObjects = Physics.OverlapSphere(selectionPreview.transform.position, sphereRadius, input.gridLayerMask);
        return interstectingObjects.Length != 0 ? false : true;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.matrix = Matrix4x4.TRS(selectionPreview.transform.position, selectionPreview.transform.rotation, new Vector3((grid.cellGap.x + grid.cellSize.x) * maxGridLength, 1f, 1f));
    //     Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    // }
}
