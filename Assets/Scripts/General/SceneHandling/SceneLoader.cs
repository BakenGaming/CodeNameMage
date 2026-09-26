using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneDataSO[] sceneDataSOArray;
    private Dictionary<string, int> sceneIDToIndexMap = new Dictionary<string, int>();

    void Awake()
    {
        GameManager.i.sceneLoader = this;
        PopulateSceneMapping();
    }
    private void PopulateSceneMapping()
    {
        foreach (var sceneDataSO in sceneDataSOArray)
        {
            sceneIDToIndexMap[sceneDataSO.uniqueName] = sceneDataSO.sceneIndex;
        }
    }
    public void LoadSceneByIndex(string savedSceneID)
    {
        if(sceneIDToIndexMap.TryGetValue(savedSceneID, out int sceneIndex))
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else Debug.LogError($"No scene found for ID: {savedSceneID}");
    }
    public async Task LoadSceneByIndexAsync(string savedSceneID)
    {
        if(sceneIDToIndexMap.TryGetValue(savedSceneID, out int sceneIndex))
        {
            AsyncOperation asyncLoad =  SceneManager.LoadSceneAsync(sceneIndex);
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                if(asyncLoad.progress >= .9f)
                {
                    asyncLoad.allowSceneActivation = true;
                    break;
                }
                await Task.Yield();
            }
        }
        else Debug.LogError($"No scene found for ID: {savedSceneID}");
    }

}
