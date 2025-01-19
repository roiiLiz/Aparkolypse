using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Friendly Unit Stats", fileName = "Friendly Unit Stats")]
public class FriendlyUnitStats : ScriptableObject
{
    public int healthAmount;
    public int damageAmount;
    public int range;
    public float attackRate;
    public float knockbackForce;
    public AttackType attackType;
    // public float overclockDuration;
    // public OverclockStrategy overclockStrategy;
    public int creditCost;
}
