using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Walking,
    Attacking
}

public class EnemyUnit : MonoBehaviour
{
    [SerializeField]
    private EnemyStats stats;
    [SerializeField]
    private HealthComponent healthComponent;
    [SerializeField]
    private AttackCollider attackRange;

    private EnemyState currentState;
    private float movementSpeed;
    private int attackDamageAmount;
    private float attackRate;
    // private int 

    private bool canAttack = true;

    private void Start()
    {
        currentState = stats.startingState;
        movementSpeed = stats.movementSpeedInSeconds;
        attackDamageAmount = stats.damageAmount;
        attackRate = stats.attackRate;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Walking:
                TickWalkingState();
                break;
            case EnemyState.Attacking:
                TickAttackState();
                break;
            default:
                break;
        }
    }

    private void TickWalkingState()
    {
        // walking logic
        transform.Translate(Vector3.left * Time.deltaTime * movementSpeed, Space.World);

        if (attackRange.enemyIsInRange)
        {
            currentState = EnemyState.Attacking;
            // StartCoroutine(BeginAttackCooldown());
        }
    }

    private void TickAttackState()
    {
        if (canAttack)
        {
            BeginAttack();
        }

        if (attackRange.enemyIsInRange == false)
        {
            currentState = EnemyState.Walking;
        }
    }

    private void BeginAttack()
    {
        var unitAttack = new Attack();

        unitAttack.damageAmount = attackDamageAmount;
        // unitAttack.knockbackForce = knockbackForce;
        // TODO: See if this works
        // unitAttack.attackDirection = Vector3.forward;

        for (int i = attackRange.enemies.Count - 1; i > -1; i--)
        {
            GameObject enemy = attackRange.enemies[i];
            enemy.GetComponent<HealthComponent>().Damage(unitAttack);
        }

        canAttack = false;
        StartCoroutine(BeginAttackCooldown());
    }

    private IEnumerator BeginAttackCooldown()
    {
        float t = 0.0f;
        float cooldownDuration = 1.0f / attackRate;

        while (t < cooldownDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
