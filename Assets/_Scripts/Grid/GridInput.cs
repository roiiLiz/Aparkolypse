using System;
using UnityEngine;

public class GridInput : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private LayerMask groundLayerMask;
    [field: SerializeField]
    public LayerMask gridLayerMask { get; private set; }
    [SerializeField]
    private float rayDistance = 100f;

    private Vector3 targetPosition;
    public bool allowPlacement { get; private set; } = true;

    public Vector3 GetGridPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = playerCamera.nearClipPlane;

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        RaycastHit rayHit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out rayHit, rayDistance, groundLayerMask))
        {
            targetPosition = rayHit.point;
            allowPlacement = true;
        } else
        {
            allowPlacement = false;
        }

        return targetPosition;
    }

    public bool GetGridInput()
    {
        if (allowPlacement)
        {
            return Input.GetMouseButtonDown(0);
        } else
        {
            return false;
        }
    }
}
