using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private List<UnitType> ignoreTypes = new List<UnitType>();
    [SerializeField]
    private Sprite friendlyBulletSprite, enemyBulletSprite;

    public int damageAmount;
    private UnitType unitType;
    private Vector3 bulletDirection;

    private Image image => GetComponentInChildren<Image>();

    public void SetType(UnitType type)
    {
        if (unitType != type)
        {
            unitType = type;

            if (unitType == UnitType.Friendly)
            {
                image.sprite = friendlyBulletSprite;
            } else if (unitType == UnitType.Enemy)
            {
                image.sprite = enemyBulletSprite;
            }
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
        } else if (unitType == UnitType.Enemy || unitType == UnitType.Boss)
        {
            bulletDirection = Vector3.left;
        }

        transform.Translate(bulletDirection * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealthComponent healthComponent = collision.GetComponent<HealthComponent>();
        // Debug.Log("Hello");

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
