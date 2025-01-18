using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public int healthAmount;
    public int damageAmount;
    public int damageRange;
    public float attackRate;
    public int movementSpeed;
    public int creditsOnDeath;
}