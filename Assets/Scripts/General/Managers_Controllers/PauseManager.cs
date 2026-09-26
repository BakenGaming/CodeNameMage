using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused {get; private set;}
    void Awake()
    {
        GameManager.i.pauseManager = this;
        GameManager.i.Input.OnMenuOpenEvent += HandlePauseInput;
        UnPauseGame();
    }
    void OnDisable()
    {
        GameManager.i.Input.OnMenuOpenEvent -= HandlePauseInput;
    }
    public void HandlePauseInput()
    {
        if(isPaused) UnPauseGame();
        else PauseGame();
    }
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        GameManager.i.Input.DisablePlayerControls();
        Debug.Log($"Pausing : TimeScale = {Time.timeScale}");
    }
    private void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        GameManager.i.Input.EnablePlayerControls();
        Debug.Log($"UnPausing : TimeScale = {Time.timeScale}");
    }
    public bool GetIsPaused(){return isPaused;}
}
