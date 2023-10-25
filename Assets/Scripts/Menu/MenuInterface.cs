using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInterface : MonoBehaviour
{
    [SerializeField] GameObject options;

    [SerializeField] private Toggle soundToggle, musicToggle;
    [SerializeField] private Slider soundChange, musicChange;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Continue()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    { 
        options.SetActive(!options.activeSelf);
    }
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void SoundCheck()
    {
        soundChange.gameObject.SetActive(soundToggle.isOn);
        MenuManager.Instance.soundsOn = soundToggle.isOn;
    }
    public void MusicCheck()
    {
        musicChange.gameObject.SetActive(musicToggle.isOn);
        MenuManager.Instance.musicOn = musicToggle.isOn;
    }
    public void SoundChange()
    {
        MenuManager.Instance.soundVolume = soundChange.value;
    }
    public void MusicChange()
    {
        MenuManager.Instance.musicVolume = musicChange.value;
    }


}
