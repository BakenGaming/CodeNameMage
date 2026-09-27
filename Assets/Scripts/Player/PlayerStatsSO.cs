using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName ="Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Standard Stats")]
    public Sprite SPRITE;
    public GameObject HEALTHBAR;
    public int HP;
    [Header("Movement")]
    public PlayerMovementStats movementStats;
    [Header("Attack Stats")]
    public int ATK;
    public float CRIT;
}
