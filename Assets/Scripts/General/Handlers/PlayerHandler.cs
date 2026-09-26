using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum MovementType
{
    topDownStandard, topDownShip, platformer, grid
}
public class PlayerHandler : MonoBehaviour
{   
    #region Variables and Setup
    [Header("Player Setup")]
    [HideInInspector] public InputReader Input;
    public PlayerStatsSO Stats;
    public SpriteRenderer playerSprite;
    public bool useHealthBar;
    private Camera mainCam;
    private Vector3 offset = new Vector3(0f,.75f,0f);
    private GameObject healthBarGraphic;
    private Slider healthValueSlider;
    private HealthSystem _healthSystem;
    [HideInInspector] public StatSystem Statsystem {get; private set;}
    //remove start if spawning player in, call initialize
    void Awake()
    {
        GameManager.i.playerGO = this.gameObject;
    }
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        Input = GameManager.i.Input;
        Statsystem = new StatSystem(Stats);
        mainCam = Camera.main;
        if(useHealthBar)
        {
            healthBarGraphic = Instantiate(Stats.HEALTHBAR);
            healthBarGraphic.transform.SetParent(gameObject.transform);
            healthValueSlider = healthBarGraphic.transform.Find("Slider").GetComponent<Slider>();
        }
        _healthSystem = new HealthSystem(Stats.HP);
        transform.AddComponent<PlayerMovement_topdown>();
        GetComponent<IPlayerMovementHandler>().Initialize();
    }
    #endregion
    #region Handle Player Functions
    public void HandleDeath()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateHealth()
    {
        throw new System.NotImplementedException();
    }
    private void UpdateHealthBar()
    {
        healthBarGraphic.transform.rotation = mainCam.transform.rotation;
        healthBarGraphic.transform.position = transform.position + offset; 
    }
    public void TakeDamage(int _damage, bool _isCrit)
    {
        
    }

    #endregion
    void Update()
    {
        if(useHealthBar) UpdateHealthBar();
    }
    #region Saving and Loading
    public void Save(ref PlayerSaveData data)
    {
        data.position = transform.position;
    }
    public void Load(PlayerSaveData data)
    {
        transform.position = data.position;
    }
    #endregion
}
[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 position;
}
