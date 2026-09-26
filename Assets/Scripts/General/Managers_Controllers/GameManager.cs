using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager _i;
    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool isSpawningPlayer;
    public InputReader Input;
    public GameObject playerGO {get; set;}
    public InventoryManager inventoryManager {get; set;}
    public PauseManager pauseManager {get; set;}
    public StaticVariables staticVariables {get; set;}
    //References for saving
    public PlayerHandler player {get; set;}
    public SceneData sceneData {get; set;}
    public SceneLoader sceneLoader {get; set;}
    private Dictionary<string, InputActionMap> actionMapDictionary = new Dictionary<string, InputActionMap>();
    //any other items that the save manager would need access to would go here as variables
    //set script execution order with gamemanager first
    #endregion
    
    #region Initialize
    private void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);
        _i = this;  
        Initialize();
    }

    private void Initialize() 
    {
        if(isSpawningPlayer) SpawnPlayerObject();
    }

    private void SpawnPlayerObject()
    {
        GameObject playerGOref = Instantiate(GameAssets.i.pfPlayerObject, spawnPoint);
        playerGOref.transform.parent = null;
    }
    #endregion
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
}
