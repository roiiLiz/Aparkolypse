using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
}

public class Attack
{
    public int damageAmount;
    public float knockbackForce;
    public Vector2 attackDirection;

    public Attack() { }
}