using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task:MonoBehaviour
{
    public new string name;
    public string description;
    public bool completed;


    public delegate void OnTask();
    public virtual event OnTask SayTask;
    [SerializeField] protected TaskManager taskManager;

    private void Start()
    {
       // taskManager = GetComponent<TaskManager>();
    }
    public virtual void ToDo()
    {
        Debug.Log("task");
    }
    public virtual void Say()
    {
        Debug.Log(description);
    }
    public void OnComplete()
    {
        completed = true;
        taskManager.SetCurrentTask();

    }
}
