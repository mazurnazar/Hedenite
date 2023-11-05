using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToWait;
    [SerializeField] private bool timerOut;
    [SerializeField] public bool timerActive;
    public bool TimerOut { get => timerOut; }
    public delegate void OnTimer();
    public event OnTimer TimerFinished;
    [SerializeField] int seconds;
    [SerializeField] TaskManager taskManager;
    
    [SerializeField] JournalManager journal_Manager;
    Coroutine currentCoroutine;
    private void Start()
    {
        Say.DialogueStarted += StopTimer;
        Say.DialogueStopped += StartTimer;

        JournalManager._instance.JournalOn += StartTimerEvent;
       // JournalManager._instance.JournalOn += StartTimer;
        /*
        currentCoroutine = StartCoroutine( StartTimerCountdown());
        StartTimer();*/
    }
    public void StartTimerEvent(bool activate)
    {
        if (!activate) StartTimer();
        else StopTimer();
    }
    public void StartTimer()
    {
        //Debug.Log(timerActive);
        if (!timerActive) return;
        seconds = 0;
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTimerCountdown());

    }
    public IEnumerator StartTimerCountdown()
    {
       // Debug.Log("timer started");
        yield return new WaitForSeconds(timeToWait);
        timerOut = true;
       // timerActive = false;
        TimerFinished?.Invoke();
    }
    public void StopTimer()
    {
        if(currentCoroutine!=null)
        StopCoroutine(currentCoroutine);
        //Debug.Log("timer stopped");
        //timerActive = false;
    }
    private void OnDestroy()
    {
        Say.DialogueStarted -= StopTimer;
        Say.DialogueStopped -= StartTimer;
        JournalManager._instance.JournalOn -= StartTimerEvent;
    }
}
