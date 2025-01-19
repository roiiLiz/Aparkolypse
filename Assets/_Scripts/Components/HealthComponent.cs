using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Friendly,
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
    [SerializeField]
    private DamageFlashComponent damageFlashComponent;

    private int currentHealth;

    public int MaxHealth { get { return maxHealth; } set { SetHealth(value); } }
    public int CurrentHealth { get { return currentHealth; } }
    public UnitType UnitType { get { return unitType; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetHealth(int incomingAmount)
    {
        maxHealth = incomingAmount;
        currentHealth = maxHealth;
        CheckHealth();
    }

    public void Damage(Attack attack)
    {
        currentHealth -= attack.damageAmount;
        if (damageFlashComponent != null) 
        {
            damageFlashComponent.BeginDamageFlash();
        }
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            if (deathComponent != null)
            {
                deathComponent.Die(this);
            }
        }
    }
}
