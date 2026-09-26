using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    public void SetMasterVolume(float volume)
    {
        GameManager.i.staticVariables.GetMusicMixer().audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        GameManager.i.staticVariables.GetSFXMixer().audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        GameManager.i.staticVariables.GetMusicMixer().audioMixer.SetFloat("MusicVolume", volume);
    }
}
