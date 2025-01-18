using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Enemy Death Component", fileName = "Enemy Death Component")]
public class EnemyDeathComponent : DeathComponent
{
    public static event Action enemyDied;
    public static event Action<int> grantDeathCredits;
    public EnemyStats enemyStats;

    public override void Die(MonoBehaviour context)
    {
        context.gameObject.SetActive(false);
        enemyDied?.Invoke();
        grantDeathCredits?.Invoke(enemyStats.creditsOnDeath);
    }
}
