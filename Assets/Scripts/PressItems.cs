using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PressItems : Task
{
    public override event OnTask SayTask;
    [SerializeField] private List<Item> items;
    [SerializeField] public NewTask newTask;
    [SerializeField] ScenesManager sceneManager;
    public override void ToDo()
    {
        SayTask?.Invoke();
    }
    private void Awake()
    {
        SayTask += SayInfo;
        sceneManager = GameObject.Find("GameManager").GetComponent<ScenesManager>();
        Scene scene = sceneManager.CurrentScene;
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
    }
    private void Start()
    {
        List<Item> allSceneItems = sceneManager.CurrentScene.allItems;
        items = new List<Item>();
        description = newTask.description;
        foreach (var item in newTask.itemNames)
        {
            for (int i = 0; i < allSceneItems.Count; i++)
            {
                if (allSceneItems[i].name == item)
                {
                    items.Add(allSceneItems[i]);
                }
            }
           
        }
        foreach (var item in items)
        {
            item.OnPress += CheckPressed;
        }
        taskManager.GetTasks(this);
    }
    public void SayInfo()
    {
        if (items.Count == 1)
        {
            Debug.Log(description + items[0].ItemName);
        }
        else if (items.Count > 0)
        {
            Debug.Log(description);
        }
    }
    public void CheckPressed()
    {

        foreach (var item in items)
        {
            //Debug.Log(item.Clicked);
            if (!item.Clicked) return;
            else item.OnPress -= CheckPressed;
        }
        completed = true;
        OnComplete();
    }

}
