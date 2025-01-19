using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool enemyIsInRange { get; private set; } = false;
    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    public List<UnitType> ignoreTypes = new List<UnitType>();
    // private BoxCollider attackCollider;
    // private int enemyCount = 0;

    private void OnEnable() { FriendlyTowerDeathComponent.friendlyUnitDied += CheckForFriendlyUnit; }
    private void OnDisable() { FriendlyTowerDeathComponent.friendlyUnitDied -= CheckForFriendlyUnit; } 

    private void CheckForFriendlyUnit(GameObject friendlyUnit)
    {
        for (int i = enemies.Count - 1; i > -1; i--)
        {
            GameObject unit = enemies[i];

            if (unit == null || unit == friendlyUnit)
            {
                Debug.Log("Hello from if statement");
                enemies.RemoveAt(i);
            }
        }

        CheckForEnemies();
    }

    private void Start()
    {
        // attackCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();

        if (healthComponent != null && !ignoreTypes.Contains(healthComponent.UnitType))
        {
            Debug.Log($"Adding enemy: {collision.gameObject.name}");
            enemies.Add(collision.gameObject);
            // enemyCount++;
            CheckForEnemies();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        enemies.Remove(collision.gameObject);
        // enemyCount--;
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        enemyIsInRange = enemies.Count <= 0 ? false : true;
    }
}
