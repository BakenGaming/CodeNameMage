using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName ="Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Setup")]
    public GameObject HEALTHBAR;

    [Header("Base Stats")]
    public int HP;
    public float SPEED;
    public int ATK;
    public float CRIT;
    public float LUCK;
}
