using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToWait;
    [SerializeField] private bool timerOut;
    [SerializeField] private bool timerActive;
    public bool TimerOut { get => timerOut; }
    public delegate void OnTimer();
    public event OnTimer TimerFinished;
    [SerializeField] int seconds;
    
    [SerializeField] JournalManager journal_Manager;
    Coroutine currentCoroutine;
    private void Start()
    {
        Say.DialogueStarted += StopTimer;
        Say.DialogueStopped += StartTimer;

        JournalManager._instance.JournalOn += StartTimerEvent;
       // JournalManager._instance.JournalOn += StartTimer;

        currentCoroutine = StartCoroutine( StartTimerCountdown());
        StartTimer();
    }
    public void StartTimerEvent(bool activate)
    {
        if (!activate) StartTimer();
        else StopTimer();
    }
    public void StartTimer()
    {
        seconds = 0;
        timerActive = true;
        StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTimerCountdown());

    }
    public IEnumerator StartTimerCountdown()
    {
        yield return new WaitForSeconds(timeToWait);
        timerOut = true;
        timerActive = false;
        TimerFinished?.Invoke();
    }
    public void StopTimer()
    {
        StopCoroutine(currentCoroutine);
        timerActive = false;
    }
    private void OnDestroy()
    {
        Say.DialogueStarted -= StopTimer;
        Say.DialogueStopped -= StartTimer;
        JournalManager._instance.JournalOn -= StartTimerEvent;
    }
}
