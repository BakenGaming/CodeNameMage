using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class SaveSystem
{
    //Setup a struct in each of the scripts that saving is needed
    //The struct is the data that needs to be saved
    //Create the save and load in each of those that saves/loads the data
    private static SaveData saveData = new SaveData();

    [System.Serializable]
    public struct SaveData
    {
        //Add the data structs for each of the scripts that have save data
        public PlayerSaveData playerData;
        public SceneSaveData sceneData;
    }
    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }
    public static void Save()
    {
        HandleSaveData();
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(saveData));
    }
    #region Async Save/Load
    public static async Task SaveAsynchronously()
    {
        await SaveAsync();
    }
    private static async Task SaveAsync()
    {
        HandleSaveData();
        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(saveData));
    }
    public static async Task LoadAsync()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        await HandleLoadDataAsync();
        GameManager.i.player.Load(saveData.playerData);
    }
    private static async Task HandleLoadDataAsync()
    {
        await GameManager.i.sceneData.LoadAsync(saveData.sceneData);
        await GameManager.i.sceneData.WaitForSceneToFullyLoad();
    }
    #endregion
    private static void HandleSaveData()
    {
        //call saves in all scripts that need saving
        GameManager.i.player.Save(ref saveData.playerData);
        GameManager.i.sceneData.Save(ref saveData.sceneData);
    }
    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());
        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        HandleLoadData();
        
    }
    private static void HandleLoadData()
    {
        //call loads in all scripts that need loading
        GameManager.i.player.Load(saveData.playerData);
        GameManager.i.sceneData.Load(saveData.sceneData);
    }
}
