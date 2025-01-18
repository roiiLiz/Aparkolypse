using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy,
    Boss
}

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private DeathComponent deathComponent;
    [SerializeField]
    private UnitType unitType;
    // [SerializeField]
    // private DamageFlashComponent damageFlashComponent;

    private int currentHealth;

    public int MaxHealth { get { return maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } }
    public UnitType UnitType { get { return unitType; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(Attack attack)
    {
        currentHealth -= attack.damageAmount;

        if (currentHealth <= 0)
        {
            if (deathComponent != null) 
            {
                deathComponent.Die(this);
            }
        }
    }
}
