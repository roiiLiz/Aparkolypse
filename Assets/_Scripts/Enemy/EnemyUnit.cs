using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Transform[] firingPoints;
    [SerializeField]
    private GameObject bulletPrefab;
    [Header("Boss Stats")]
    [SerializeField]
    private GameObject cartPrefab;

    private EnemyState currentState;
    private float movementSpeed;
    private int attackDamageAmount;
    private float attackRate;
    private AttackType attackType;
    private UnitType unitType;

    private bool canAttack = true;

    private void Start()
    {
        currentState = stats.startingState;
        movementSpeed = stats.movementSpeedInSeconds;
        attackDamageAmount = stats.damageAmount;
        attackRate = stats.attackRate;
        attackType = stats.attackType;
        unitType = stats.unitType;

        // if (unitType == UnitType.Boss)
        // {
        //     InitializeFiringPoints();
        // }
    }

    private void InitializeFiringPoints()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("BossFiringPoints");
        foreach (GameObject point in points)
        {
            firingPoints.Append(point.transform);
        }
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
                    if (unitType == UnitType.Enemy)
                    {
                        var bullet = Instantiate(bulletPrefab, firingPoints[0].transform.position, Quaternion.identity);
                        Bullet bulletComponent = bullet.GetComponent<Bullet>();

                        bulletComponent.damageAmount = attackDamageAmount;
                        bulletComponent.AddIgnoreType(UnitType.Enemy);
                        bulletComponent.AddIgnoreType(UnitType.Boss);
                        bulletComponent.SetType(healthComponent.UnitType);
                    } else if (unitType == UnitType.Boss)
                    {
                        for (int i = 0; i < firingPoints.Length; i++)
                        {
                            var cart = Instantiate(cartPrefab, firingPoints[i].transform.position, Quaternion.identity);
                            Bullet cartDamage = cart.GetComponent<Bullet>();

                            cartDamage.damageAmount = attackDamageAmount;
                            cartDamage.AddIgnoreType(UnitType.Enemy);
                            cartDamage.AddIgnoreType(UnitType.Boss);
                            cartDamage.SetType(healthComponent.UnitType);
                        }
                    }
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
