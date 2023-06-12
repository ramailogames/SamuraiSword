using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player Data")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed;
    public float moveSpeedWhileAttacking;
    public float jumpForce;
    public float dashSpeed;
    public float groundCheckRadius;
    public float enemyCheckRadius;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    public float damageAmount;
}
