using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Friendly Tower Death Component", fileName = "Friendly Tower Death Component")]
public class FriendlyTowerDeathComponent : DeathComponent
{
    public static event Action friendlyUnitDied;

    public override void Die(MonoBehaviour context)
    {
        context.gameObject.SetActive(false);
        friendlyUnitDied?.Invoke();
    }
}
