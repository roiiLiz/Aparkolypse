using System;
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
    [Header("Required Components")]
    [SerializeField]
    private int maxHealth = 10;
    [SerializeField]
    private DeathComponent deathComponent;
    [SerializeField]
    private UnitType unitType;
    [SerializeField]
    private DamageFlashComponent damageFlashComponent;
    [Header("Castle / Player Stats")]
    [SerializeField]
    private int firstLifeThreshold;
    [SerializeField]
    private int secondLifeThreshold;
    [SerializeField]
    private int thirdLifeThreshold;

    public static event Action OnDamageThresholdReached;

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

        TryKnockback(attack.knockbackForce, attack.attackDirection);

        if (damageFlashComponent != null) 
        {
            damageFlashComponent.BeginDamageFlash();
        }
        CheckHealth();
    }

    private void TryKnockback(float knockbackForce, Vector3 attackDir)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 knockbackDir = attackDir - transform.position;
            rb.AddForce(knockbackDir.normalized * knockbackForce);
        }
    }

    private void CheckHealth()
    {
        if (unitType == UnitType.Player)
        {
            if (currentHealth <= firstLifeThreshold)
            {
                OnDamageThresholdReached?.Invoke();
            } else if (currentHealth <= secondLifeThreshold)
            {
                OnDamageThresholdReached?.Invoke();
            } else if (currentHealth <= thirdLifeThreshold)
            {
                OnDamageThresholdReached?.Invoke();
            }
        }


        if (currentHealth <= 0)
        {
            if (deathComponent != null)
            {
                deathComponent.Die(this);
            }
        }
    }
}
