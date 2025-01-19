using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Components/Player Death Component", fileName = "Player Death Component")]
public class PlayerDeathComponent : DeathComponent
{
    public static event Action playerDied;

    public override void Die(MonoBehaviour context)
    {
        
        // context.gameObject.SetActive(false);
        playerDied?.Invoke();
    }
}
