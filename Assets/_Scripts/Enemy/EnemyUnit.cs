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
    [Header("Enemy Stats")]
    [SerializeField]
    private EnemyStats stats;
    [SerializeField]
    private HealthComponent healthComponent;
    [SerializeField]
    private AttackCollider attackRange;
    [Header("Ranged Unit Variables")]
    [SerializeField]
    private Transform firingPoint;
    [SerializeField]
    private GameObject bulletPrefab;

    private EnemyState currentState;
    private float movementSpeed;
    private int attackDamageAmount;
    private float attackRate;
    private AttackType attackType;

    private bool canAttack = true;

    private void Start()
    {
        currentState = stats.startingState;
        movementSpeed = stats.movementSpeedInSeconds;
        attackDamageAmount = stats.damageAmount;
        attackRate = stats.attackRate;
        attackType = stats.attackType;
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
        BeginAttack();

        if (attackRange.enemyIsInRange == false && attackType != AttackType.Ranged)
        {
            currentState = EnemyState.Walking;
        }
    }

    private void BeginAttack()
    {
        if (canAttack)
        {
            canAttack = false;
            switch (attackType)
            {
                case AttackType.Melee:
                    var meleeAttack = new Attack();

                    meleeAttack.damageAmount = attackDamageAmount;

                    for (int i = attackRange.enemies.Count - 1; i >= 0; i--)
                    {
                        GameObject enemy = attackRange.enemies[i];
                        enemy.GetComponent<HealthComponent>().Damage(meleeAttack);
                    }
                    
                    break;

                case AttackType.Ranged:
                    var bullet = Instantiate(bulletPrefab, firingPoint.transform.position, Quaternion.identity);
                    Bullet bulletComponent = bullet.GetComponent<Bullet>();

                    bulletComponent.damageAmount = attackDamageAmount;
                    bulletComponent.AddIgnoreType(UnitType.Enemy);
                    bulletComponent.AddIgnoreType(UnitType.Boss);
                    bulletComponent.SetType(healthComponent.UnitType);
                    break;

                default:
                    break;
            }

            StartCoroutine(BeginAttackCooldown());
        }
    }

    private IEnumerator BeginAttackCooldown()
    {
        Debug.Log("Beginning attack cooldown");
        float t = 0.0f;
        float attackCooldown = 1.0f / attackRate;

        while (t < attackCooldown)
        {
            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Finished attack cooldown");
        canAttack = true;
    }
}
