using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class ScenesManager : MonoBehaviour
{
    [SerializeField] private List<Scene> scenes;
    [SerializeField] private Scene currentScene;
    [SerializeField] PressItemsTasks pressItemsTasks;
    public Scene CurrentScene { get => currentScene; }

    public delegate void SceneNext();
    public event SceneNext MoveToScene;

    private void Awake()
    {
        SetVariable.MoveToScene += SetCurrentScene;
    }
    
    public void SetCurrentScene(string scene)
    {
        foreach (var item in scenes)
        {
            if (item.name == scene)
            {
                currentScene = item;
                currentScene.currentStage = 1;
                //Debug.Log(currentScene);
                AudioManager.Instance.ChangeAudioMixerGroup(currentScene.mixerGroup);
            }
        }
        if (pressItemsTasks != null)
        {
            int counter = 0;
            foreach (var item in pressItemsTasks.tasks)
            {
                if (!item.completed)
                {
                    PressItems pressItems = currentScene.gameObject.AddComponent<PressItems>();
                    pressItems.newTask = pressItemsTasks.tasks[counter];
                    counter++;
                }
                else counter++;
                
            }
            /*
            int counter = 0;
            foreach (var item in currentScene.gameObject.GetComponents<PressItems>())
            {
                item.newTask = pressItemsTasks.tasks[counter];
                counter++;

            }*/
        }
        MoveToScene?.Invoke();
    }
    private void OnDestroy()
    {
        SetVariable.MoveToScene -= SetCurrentScene;
    }

}
