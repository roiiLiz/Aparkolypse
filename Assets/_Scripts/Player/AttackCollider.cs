using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool enemyIsInRange { get; private set; } = false;
    public List<GameObject> enemies = new List<GameObject>();
    private BoxCollider attackCollider;
    // private int enemyCount = 0;

    private void OnEnable() { FriendlyTowerDeathComponent.friendlyUnitDied += CheckForFriendlyUnit; }
    private void OnDisable() { FriendlyTowerDeathComponent.friendlyUnitDied -= CheckForFriendlyUnit; } 

    private void CheckForFriendlyUnit(GameObject friendlyUnit)
    {
        foreach (GameObject unit in enemies.Where(i => i != null).ToList())
        {
            if (unit == friendlyUnit)
            {
                print("Unit found, removing from list");
                enemies.Remove(unit);
            }
        }
    }

    private void Start()
    {
        attackCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();

        if (healthComponent != null)
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
