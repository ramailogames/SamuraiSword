using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public Material whiteMat;
    public float knockBackForce;
}
