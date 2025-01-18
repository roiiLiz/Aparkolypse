using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [Header("Grid Selection")]
    [SerializeField]
    private GameObject selectionPreview, towerPrefab;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GridInput input;
    [Header("Sphere Overlap")]
    [SerializeField]
    private float sphereRadius = 1f;

    public static event Action OnTileClicked;
    // Update is called once per frame
    private void Update()
    {
        Vector3 selectedPosition = input.GetGridPosition();

        Vector3Int gridCellPosition = grid.WorldToCell(selectedPosition);

        selectionPreview.transform.position = grid.GetCellCenterWorld(gridCellPosition);

        if (input.GetGridInput())
        {
            if (GridTileIsEmpty())
            {
                Instantiate(towerPrefab, selectionPreview.transform.position, Quaternion.identity, grid.gameObject.transform);
            }

            OnTileClicked?.Invoke();
        }
    }

    private bool GridTileIsEmpty()
    {
        Collider[] interstectingObjects = Physics.OverlapSphere(selectionPreview.transform.position, sphereRadius, input.gridLayerMask);
        return interstectingObjects.Length != 0 ? false : true;
    }
}
