using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private List<UnitType> ignoreTypes = new List<UnitType>();

    public int damageAmount;
    private UnitType unitType;
    private Vector3 bulletDirection;

    public void SetType(UnitType type)
    {
        if (unitType != type)
        {
            unitType = type;
        }
    }

    public void AddIgnoreType(UnitType type)
    {
        if (!ignoreTypes.Contains(type))
        {
            ignoreTypes.Add(type);
        }
    }

    private void Update()
    {
        if (unitType == UnitType.Friendly)
        {
            bulletDirection = Vector3.right;
        } else if (unitType == UnitType.Enemy)
        {
            bulletDirection = Vector3.left;
        }

        transform.Translate(bulletDirection * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();
        Debug.Log("Hello");

        if (healthComponent != null && !ignoreTypes.Contains(healthComponent.UnitType))
        {
            Debug.Log("Helloooo");
            var bulletAttack = new Attack();

            bulletAttack.damageAmount = damageAmount;

            healthComponent.Damage(bulletAttack);

            Destroy(gameObject);
        }
    }
}
