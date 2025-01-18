using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public enum UnitState
{
    Default,
    Damaged,
    Overclocked
}

public class FriendlyUnit : MonoBehaviour
{
    [SerializeField]
    private FriendlyUnitStats stats;
    [SerializeField]
    private HealthComponent healthComponent;
    [SerializeField]
    private AttackCollider attackRange;
    [SerializeField]
    private Material supportMaterial, headMaterial;

    private MeshRenderer mesh => GetComponent<MeshRenderer>();

    private UnitState unitState = UnitState.Default;
    private int attackDamage;
    private bool canAttack = true;
    private float knockbackForce;
    private float attackRate;

    // private void OnEnable() { GridSelector.OnDetermineHead += ChangeMesh; }
    // private void OnDisable() { GridSelector.OnDetermineHead -= ChangeMesh; }

    private void Start()
    {
        healthComponent.MaxHealth = stats.healthAmount;
        attackRange.GetComponent<BoxCollider>().size = new Vector3(stats.range, 1f, stats.range);
        attackDamage = stats.damageAmount;
        knockbackForce = stats.knockbackForce;
        attackRate = stats.attackRate;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Input");
            StartCoroutine(BeginAttackCooldown());
        }

        switch (unitState)
        {
            case UnitState.Default:
                break;
            case UnitState.Damaged:
                break;
            case UnitState.Overclocked:
                break;
            default:
                break;
        }
    }

    public void SetCartType(bool isHeadCart)
    {
        mesh.material = isHeadCart ? headMaterial : supportMaterial;
    }

    private void BeginAttack()
    {
        var unitAttack = new Attack();

        unitAttack.damageAmount = attackDamage;
        unitAttack.knockbackForce = knockbackForce;
        // TODO: See if this works
        unitAttack.attackDirection = Vector3.forward;

        foreach (GameObject enemy in attackRange.enemies)
        {
            enemy.GetComponent<HealthComponent>().Damage(unitAttack);
        }

        canAttack = false;
        StartCoroutine(BeginAttackCooldown());
    }

    // private void OnTriggerEnter(Collider collider)
    // {
    //     HealthComponent healthComponent = collider.GetComponent<HealthComponent>();

    //     if (healthComponent != null && canAttack)
    //     {
    //         var unitAttack = new Attack();

    //         unitAttack.damageAmount = attackDamage;
    //         unitAttack.knockbackForce = knockbackForce;
    //         // TODO: See if this works
    //         unitAttack.attackDirection = Vector3.forward;

    //         healthComponent.Damage(unitAttack);

    //         canAttack = false;
    //         StartCoroutine(BeginAttackCooldown());
    //     }
    // }

    private IEnumerator BeginAttackCooldown()
    {
        float t = 0.0f;
        float attackCooldown = 1.0f / attackRate;

        while (t > attackCooldown)
        {
            t += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }
}
