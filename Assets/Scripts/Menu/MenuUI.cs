using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject Menu;
    [SerializeField] Animator animator;
    [SerializeField] GameObject options;
    [SerializeField] Button menuButton;
    public delegate void StopGame(bool stop);
    public event StopGame OnStop;

    [SerializeField] private Toggle soundToggle, musicToggle;
    [SerializeField] private Slider soundChange, musicChange;
    public void ActivateMenu(bool stop)
    {
        menuButton.enabled = !stop;
        Menu.SetActive(stop);
        OnStop?.Invoke(stop);
    }
    public void Options()
    {
        options.SetActive(!options.activeSelf);
        bool activate = options.activeSelf;
        animator.SetBool("activate", activate);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
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
