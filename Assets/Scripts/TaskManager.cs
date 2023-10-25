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

    [SerializeField] private bool noTasksAvailable = false;
    public void Start()
    {
        timer.TimerFinished += NextTask;
        tasks = new List<Task>();
        // sceneManager.MoveToScene += GetTasks;

        // GetTasks();

    }
    
    public void GetTasks(Task task)
    {
        tasks.Add(task);
        completed.Add(task.completed);
        if(tasks.Count<2)
        SetCurrentTask();
    }
    
    public void SetCurrentTask()
    {
        if (tasks.Count == 0) {  return; }
        bool setTask = false;
        while (!setTask)
        {
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
            }
            else
            {
                if (currentTaskNumber < tasks.Count)
                {
                    currentTaskNumber++;
                    setTask = true;
                }
                else
                {
                    noTasksAvailable = true;
                    break;
                }
            }
        }
    }
    public void NextTask()
    {
        //Debug.Log(currentTaskDescription);
        if (noTasksAvailable) return;
        timerButton.gameObject.SetActive(true);
        flick = true;
        
        currentCoroutine = StartCoroutine(ButtonFlicker());
        /*
        foreach (var item in tasks)
        {
            if (item.completed) continue;
            item.ToDo();
            break;
        }*/
    }

    public IEnumerator ButtonFlicker()
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
