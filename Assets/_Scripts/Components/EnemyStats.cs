using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Enemy Stats", fileName = "Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public int healthAmount;
    public int damageAmount;
    // public int damageRange;
    [Tooltip("Describes the attacks per second when 'in range'")]
    public float attackRate;
    [Tooltip("Describes the amount of seconds it takes to move one grid unit")]
    public int movementSpeedInSeconds;
    public int creditsOnDeath;
    public EnemyState startingState = EnemyState.Walking;
}