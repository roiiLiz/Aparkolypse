using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool enemyIsInRange { get; private set; } = false;
    public List<GameObject> enemies = new List<GameObject>();
    private BoxCollider collider;
    // private int enemyCount = 0;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();

        if (healthComponent != null)
        {
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
