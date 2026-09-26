using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName ="Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Standard Stats")]
    public Sprite SPRITE;
    public GameObject HEALTHBAR;
    public int HP;
    public float SPEED;
    [Header("RigidBody Stats")]
    public float MASS;
    public float LINEARDAMPING;
    public float ANGULARDAMPING;
    public float GRAVITYSCALE;
    [Header("Platformer Stats")]
    public float JUMPPOWER;
    public float DASHPOWER;
    public int NUMBEROFJUMPS;
    [Header("Attack Stats")]
    public int ATK;
    public float CRIT;
}
