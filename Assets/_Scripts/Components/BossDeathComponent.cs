using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Boss Death Component", fileName = "Boss Death Component")]
public class BossDeathComponent : DeathComponent
{
    public static event Action bossDied;

    public override void Die(MonoBehaviour context)
    {
        context.gameObject.SetActive(false);
        bossDied?.Invoke();
    }
}
