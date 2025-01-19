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

    private GameplayManager gameplayManager;
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

        gameplayManager = GameObject.FindGameObjectWithTag("GameplayManager").GetComponent<GameplayManager>();
    }

    private void Update()
    {
        if (canAttack && gameplayManager.state == GameState.RIDE)
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
            canAttack = false;

            switch (attackType)
            {
                case AttackType.Melee:
                    var meleeAttack = new Attack();

                    meleeAttack.damageAmount = attackDamage;
                    meleeAttack.knockbackForce = knockbackForce;
                    // TODO: See if this works
                    meleeAttack.attackDirection = transform.position;

                    for (int i = attackRange.enemies.Count - 1; i >= 0; i--)
                    {
                        GameObject enemy = attackRange.enemies[i];
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
