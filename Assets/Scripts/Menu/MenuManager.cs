using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public bool soundsOn = true;
    public bool musicOn = true;
    [Range(0, 1)]
    public float soundVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

   

}
