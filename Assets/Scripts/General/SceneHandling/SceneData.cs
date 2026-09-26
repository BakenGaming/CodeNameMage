using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneData : MonoBehaviour
{
    public SceneDataSO sceneData;

    void Awake()
    {
        GameManager.i.sceneData = this;
    }
    public void Save(ref SceneSaveData data)
    {
        data.sceneID = sceneData.uniqueName;
    }
    public void Load(SceneSaveData data)
    {
        GameManager.i.sceneLoader.LoadSceneByIndex(data.sceneID);
    }
    public async Task LoadAsync(SceneSaveData data)
    {
        await GameManager.i.sceneLoader.LoadSceneByIndexAsync(data.sceneID);
    }
    public Task WaitForSceneToFullyLoad()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();
        UnityEngine.Events.UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null; 
        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;
        return taskCompletion.Task;
    }
}

[System.Serializable]
public struct SceneSaveData
{
    public string sceneID;
}
