using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public int HP;
    public int ATK;
    public float SPEED;
    public float CRIT;
}
