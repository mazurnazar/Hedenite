using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    protected AudioSource audioSourceAmbiance;
    protected AudioSource audioSourceSoundEffect;

    public AudioMixerGroup currentScene;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource[] audioSources = GetComponents<AudioSource>();

        audioSourceAmbiance = audioSources[0];
        audioSourceSoundEffect = audioSources[1];
        if (MenuManager.Instance != null)
        {
            TurnSounds();
            TurnAmbiance();
        }
    }
    public void TurnSounds()
    {
        audioSourceSoundEffect.enabled = MenuManager.Instance.soundsOn;
        audioSourceSoundEffect.volume = MenuManager.Instance.soundVolume;
    }
    public void TurnAmbiance()
    {
        audioSourceAmbiance.enabled = MenuManager.Instance.musicOn;
        audioSourceAmbiance.volume = MenuManager.Instance.musicVolume;
    }
    public void ChangeAudioMixerGroup(AudioMixerGroup audioMixerGroup)
    {
        if (audioMixerGroup == null) return;
        currentScene = audioMixerGroup;
        audioSourceAmbiance.outputAudioMixerGroup = currentScene;
    }

}
