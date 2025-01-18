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
        }

        return targetPosition;
    }

    public bool GetGridInput()
    {
        return Input.GetMouseButtonDown(0);
    }
}
