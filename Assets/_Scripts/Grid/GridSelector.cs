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
    [SerializeField]
    private float maxGridLength = 11f;

    public static event Action<GameObject> OnPlaceCart;

    // Update is called once per frame
    private void Update()
    {
        Vector3 selectedPosition = input.GetGridPosition();

        Vector3Int gridCellPosition = grid.WorldToCell(selectedPosition);

        selectionPreview.transform.position = grid.GetCellCenterWorld(gridCellPosition);

        if (input.GetGridInput() && GridTileIsEmpty())
        {
            // CheckAdjacentTiles();
            var cart = Instantiate(towerPrefab, selectionPreview.transform.position, Quaternion.identity, grid.gameObject.transform);
            OnPlaceCart?.Invoke(cart);
        }
    }

    // private void CheckAdjacentTiles()
    // {
    //     GameObject headCart = null;

    //     Collider[] intersectingObjects = Physics.OverlapBox(selectionPreview.transform.position, new Vector3((grid.cellSize.x + grid.cellGap.x) * maxGridLength, 1f, 1f), Quaternion.identity, input.gridLayerMask);
    //     if (intersectingObjects.Length != 0)
    //     {
    //         for (int i = 0; i > intersectingObjects.Length; i++)
    //         {
    //             Debug.Log($"Name: {intersectingObjects[i].gameObject.name}");
    //             if (i == 0)
    //             {
    //                 headCart = intersectingObjects[i].gameObject;
    //             }
                
    //             if (intersectingObjects[i].gameObject.transform.position.x > headCart.transform.position.x)
    //             {
    //                 headCart = intersectingObjects[i].gameObject;
    //             }
    //         }

    //         OnDetermineHead?.Invoke(headCart);
    //     }
    // }

    private bool GridTileIsEmpty()
    {
        Collider[] interstectingObjects = Physics.OverlapSphere(selectionPreview.transform.position, sphereRadius, input.gridLayerMask);
        return interstectingObjects.Length != 0 ? false : true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(selectionPreview.transform.position, selectionPreview.transform.rotation, new Vector3((grid.cellGap.x + grid.cellSize.x) * maxGridLength, 1f, 1f));
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
