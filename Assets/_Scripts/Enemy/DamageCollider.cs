using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    private int attackDamage;

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("Hello");
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();

        if (healthComponent != null && healthComponent.UnitType != UnitType.Player)
        {
            var colliderAttack = new Attack();

            colliderAttack.damageAmount = attackDamage;

            healthComponent.Damage(colliderAttack);

            Debug.Log($"Dealth {colliderAttack.damageAmount} damage to {collision.name}");
        }
    }
}
