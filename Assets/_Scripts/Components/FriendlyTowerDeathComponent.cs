using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Friendly Tower Death Component", fileName = "Friendly Tower Death Component")]
public class FriendlyTowerDeathComponent : DeathComponent
{
    public static event Action<GameObject> friendlyUnitDied;

    public override void Die(MonoBehaviour context)
    {
        context.gameObject.SetActive(false);
        // Destroy(context.gameObject);
        friendlyUnitDied?.Invoke(context.gameObject);
    }
}
