using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> tasks;
    
    [SerializeField] private List<bool> completed;
    [SerializeField] private Task currentTask;
    [SerializeField] private string currentTaskDescription;
    [SerializeField] private int currentTaskNumber = 0;

    [SerializeField] private Timer timer;
    [SerializeField] private Button timerButton;
    [SerializeField] private Button taskPanel;
    [SerializeField] private bool flick;
    [SerializeField] Coroutine currentCoroutine;
    [SerializeField] private ScenesManager sceneManager;

     private bool noTasksAvailable;

    public void Start()
    {
        timer.TimerFinished += NextTask;
        tasks = new List<Task>();
        // sceneManager.MoveToScene += GetTasks;

        // GetTasks();

    }
    
    public void GetTasks(Task task)
    {
        if (!task.completed)
        {
            tasks.Add(task);
            completed.Add(task.completed);
        }
        if (tasks.Count < 2)
            SetCurrentTask();
        if (!noTasksAvailable)
        {
            timer.StartTimer();
        }
    }
    
    public void SetCurrentTask()
    {

        //if (tasks.Count == 0) { noTasksAvailable = true; return; }


        timer.timerActive = true;
        noTasksAvailable = false;
        bool setTask = false;
        while (!setTask)
        {
            if (currentTaskNumber >= tasks.Count) { noTasksAvailable = true; return; }
            /*
            currentTask = tasks[currentTaskNumber];
            currentTaskDescription = currentTask.description;

            if (currentTask.completed)
            {
                Debug.Log(currentTaskDescription);
                if (currentTaskNumber < tasks.Count)
                    currentTaskNumber++;
                else
                {
                    noTasksAvailable = true;
                    break;
                }
            }*/
            else
            {
                //Debug.Log("here");
                noTasksAvailable = false;
                currentTask = tasks[currentTaskNumber];
                currentTaskDescription = currentTask.description;

                currentTaskNumber++;
                setTask = true;/*
                if (currentTaskNumber < tasks.Count)
                {
                    currentTaskNumber++;
                    setTask = true;
                }
                else
                {
                    noTasksAvailable = true;
                    break;
                }*/
            }
        }
    }
    
    public void NextTask()
    {

       // Debug.Log("no task " + noTasksAvailable);
        if (noTasksAvailable)
        {
            timer.StopTimer();
            timer.TimerFinished -= NextTask;
            return;
        }
        timerButton.gameObject.SetActive(true);
        flick = true;
        
        currentCoroutine = StartCoroutine(ButtonFlicker());
    }
    
    public IEnumerator ButtonFlicker()
    {
        if (flick)
        {
            float step = 0;
            while (step <= 1)
            {
                timerButton.GetComponent<Image>().color = Color.Lerp(Color.white, Color.clear, step);
                yield return new WaitForSeconds(.1f);
                step += 0.1f;
            }
            step = 0;

            while (step <= 1)
            {
                timerButton.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, step);
                yield return new WaitForSeconds(.1f);
                step += 0.1f;
            }
        }
    }
    public void TaskButtonPress()
    {
        taskPanel.gameObject.SetActive(true);
        taskPanel.GetComponentInChildren<TextMeshProUGUI>().text = currentTaskDescription;
        timerButton.GetComponent<Image>().color = Color.clear;

        flick = false;
    }
    public void TaskPanelPress()
    {
        timerButton.gameObject.SetActive(false);
        taskPanel.gameObject.SetActive(false);
        timer.StartTimer();
    }
}
