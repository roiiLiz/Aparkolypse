using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FriendlyUnitType
{
    Beetle,
    Dolphin,
    Caterpillar
}

public class UnitShop : MonoBehaviour
{
    [SerializeField]
    private List<Tower> towerPrefabs = new List<Tower>();

    public static event Action<Tower> OnSelectNewTower;

    // private Tower previousSelection;
    private Tower currentSelection;

    private void Start()
    {
        // previousSelection = currentSelection;
        currentSelection = towerPrefabs[0];
        OnSelectNewTower?.Invoke(currentSelection);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSelection = towerPrefabs[0];
            OnSelectNewTower?.Invoke(currentSelection);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSelection = towerPrefabs[1];
            OnSelectNewTower?.Invoke(currentSelection);
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSelection = towerPrefabs[2];
            OnSelectNewTower?.Invoke(currentSelection);
        }
    }
}

[Serializable]
public class Tower
{
    public GameObject prefab;
    public FriendlyUnitType unitType;
    public FriendlyUnitStats stats;
}