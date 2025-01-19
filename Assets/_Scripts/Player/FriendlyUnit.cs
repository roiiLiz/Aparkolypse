using System;
using System.Collections;
using System.Linq;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyUnit : MonoBehaviour
{
    [Header("Friendly Unit Stats")]
    [SerializeField]
    private FriendlyUnitStats stats;
    [SerializeField]
    private HealthComponent healthComponent;
    [SerializeField]
    private AttackCollider attackRange;
    // TODO: Convert from mesh to image, update carts to be seperate & merge-able
    [SerializeField]
    private Sprite supportImage, headImage;
    [SerializeField]
    private Image unitIcon;
    [Header("Ranged Unit Variables")]
    [SerializeField]
    private Transform firingPoint;
    [SerializeField]
    private GameObject bulletPrefab;

    private int attackDamage;
    private bool canAttack = true;
    private float knockbackForce;
    private float attackRate;
    private AttackType attackType;

    // private void OnEnable() { GridSelector.OnDetermineHead += ChangeMesh; }
    // private void OnDisable() { GridSelector.OnDetermineHead -= ChangeMesh; }

    private void Start()
    {
        healthComponent.MaxHealth = stats.healthAmount;
        // attackRange.GetComponent<BoxCollider>().size = new Vector3(stats.range, 1f, stats.range);
        attackType = stats.attackType;
        attackDamage = stats.damageAmount;
        knockbackForce = stats.knockbackForce;
        attackRate = stats.attackRate;
    }

    private void Update()
    {
        if (canAttack)
        {
            BeginAttack();
        }
    }

    public void SetCartType(bool isHeadCart)
    {
        unitIcon.sprite = isHeadCart ? headImage : supportImage;
    }

    private void BeginAttack()
    {
        if (canAttack)
        {
            switch (attackType)
            {
                case AttackType.Melee:
                    var meleeAttack = new Attack();

                    meleeAttack.damageAmount = attackDamage;
                    meleeAttack.knockbackForce = knockbackForce;
                    // TODO: See if this works
                    meleeAttack.attackDirection = Vector3.right;

                    foreach (GameObject enemy in attackRange.enemies)
                    {
                        enemy.GetComponent<HealthComponent>().Damage(meleeAttack);
                    }
                    break;

                case AttackType.Ranged:
                    var bullet = Instantiate(bulletPrefab, firingPoint.transform.position, Quaternion.identity);
                    Bullet bulletComponent = bullet.GetComponent<Bullet>();

                    bulletComponent.damageAmount = attackDamage;
                    bulletComponent.AddIgnoreType(UnitType.Player);
                    bulletComponent.AddIgnoreType(UnitType.Friendly);
                    bulletComponent.SetType(healthComponent.UnitType);
                    break;

                default:
                    break;
            }
        }
        
        canAttack = false;
        StartCoroutine(BeginAttackCooldown());
    }

    private IEnumerator BeginAttackCooldown()
    {
        float t = 0.0f;
        float attackCooldown = 1.0f / attackRate;

        while (t < attackCooldown)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
