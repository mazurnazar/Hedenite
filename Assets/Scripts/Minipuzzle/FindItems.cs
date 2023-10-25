using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using UnityEngine.SceneManagement;
using System;
public class FindItems : MonoBehaviour
{
    [SerializeField] private GameObject neededItems;
    private List<Riddle> riddles;
    [SerializeField] private Riddles scriptableRiddles;
    [SerializeField] private string currentNeededItem;
    private Item clickedItem;

    [SerializeField] private string rightAnswers;
    [SerializeField] private List<string> wrongAnswers;
    private int wrongAnswerNumber;

    [SerializeField] private GameObject SayDialog;
    [SerializeField] private Text sayText;
    [SerializeField] private ScenesManager sceneManager;

    [SerializeField] Camera camera;
    [SerializeField] Flowchart currentFlowchart;
    [SerializeField] SayDialog setSayDialog;
    [SerializeField] Character character;

    [SerializeField] private int timeBetweenRiddles = 2;
    private int collected;
    private bool rightItem = false;
    public static FindItems instance;
    public bool description;



    [SerializeField] private Transform inventory;
    public Vector3 center;

    private int currentRiddle = 0;

    public delegate void OnCollected(int current, int total);
    public event OnCollected Collect;

    public delegate void OnPuzzle(bool active);
    public event OnPuzzle PuzzleStart;

    [SerializeField] private bool sayRiddle;

    [SerializeField] private bool moveToInventory;
    public bool MoveToInventory { get => moveToInventory; set { moveToInventory = value; } }

    private bool puzzleStarted;

    public event Action OnMouseOver, OnMouseExit;
    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        sceneManager = GetComponent<ScenesManager>();
        List<Item> allItems = sceneManager.CurrentScene.allItems;
        SetRiddles();

        Collect?.Invoke(collected, neededItems.transform.childCount);
        SetVariable.StartPuzzleEvent += StartPuzzle;

        Say.DialogueStopped += MoveToBackpack;
    }
    public void SetRiddles()
    {
        riddles = new List<Riddle>();
        foreach (var item in scriptableRiddles.riddles)
        {
            riddles.Add(item);
        }
        SetCurrentRiddle();
    }
    public void SetCurrentRiddle()
    {
        currentNeededItem = riddles[collected].rightItem;
        rightAnswers = riddles[collected].rightAnswer;
        wrongAnswers = riddles[collected].wrongAnswers;
        wrongAnswerNumber = -1;
    }
    private void StartPuzzle()
    {
        if (puzzleStarted) return;
        puzzleStarted = true;
        StartCoroutine(SayRiddle());
        PuzzleStart?.Invoke(true);

        neededItems.SetActive(true);
    }

    public void CheckRightItem(Item itemClicked)
    {
        clickedItem = itemClicked;
        if (itemClicked.ItemName == currentNeededItem)
        {
            rightItem = true;
            ChooseAnswer(true);
        }
        else
        {
            ChooseAnswer(false);
            rightItem = false;
        }
            
    }
    public IEnumerator SayRiddle()
    {
        yield return new WaitForSeconds(timeBetweenRiddles);
        SayRiddle(riddles[currentRiddle]);
    }

    public void ChooseAnswer(bool rightItem)
    {
        description = true;
        int randomNumber = 0;
        if (rightItem)
        {
            SayAnswer(rightAnswers);
        }
        else
        {
        Random:
            randomNumber = UnityEngine.Random.Range(0, wrongAnswers.Count);
            if (randomNumber == wrongAnswerNumber) goto Random;
            SayAnswer(wrongAnswers[randomNumber]);
            wrongAnswerNumber = randomNumber;
        }

    }
    public void SayAnswer(string text)
    {
        Say say = new Say();
        if (rightItem)
        {
            moveToInventory = true;
            clickedItem.MovetoInventory = false;
            SetCurrentRiddle();
            currentRiddle++;
            if(clickedItem.AditionalItems.Length!=0)
            {
                foreach (var item in clickedItem.AditionalItems)
                {
                    StartCoroutine(item.MoveToCenter());
                }
            }
            StartCoroutine(clickedItem.MoveToCenter());
        }
        say.SayInfo(currentFlowchart, text, setSayDialog,character);
    }
     
    public void SayRiddle(Riddle riddle)
    {
        if (collected < riddles.Count)
        {
            Say say = new Say();
            say.SayInfo(currentFlowchart, riddle.riddleName, setSayDialog, character);
        }
        else SceneManager.LoadScene("Minipuzzle");
    }


    public void MoveToBackpack()
    {
        if (!moveToInventory) return;
        else moveToInventory = false;
        if(clickedItem.AditionalItems.Length!=0)
        {
            foreach (var item in clickedItem.AditionalItems)
            {
                StartCoroutine(FlyToInventory(item));
            }
        }
        StartCoroutine(FlyToInventory(clickedItem));

        collected++;
        if (collected == riddles.Count) StartCoroutine(CameraMove());
        else
        {
            rightItem = false;
            StartCoroutine(SayRiddle());
        }

    }
    public IEnumerator FlyToInventory(Item clickedItem)
    {
        Vector3 currentPos = clickedItem.transform.position;
        float step = 0;
        for (int i = 0; i < 20; i++)
        {
            clickedItem.transform.position = Vector3.Lerp(currentPos, inventory.position, step);
            step += .05f;
            yield return new WaitForSeconds(.05f);
        }
        Collect?.Invoke(collected, neededItems.transform.childCount);
        //thingsCollected.text = collected + "/" + neededItems.Count;
        clickedItem.gameObject.SetActive(false);

        if (collected < riddles.Count)
        { 
            SetCurrentRiddle();
        }    
           // currentNeededItem = riddles[collected].RiddleName;// neededItems[collected];
        Debug.Log(collected);
        
    }
    public IEnumerator CameraMove()
    {
        PuzzleStart?.Invoke(false);
        Vector3 currentPos = camera.transform.position;
        float step = 0;
        for (int i = 0; i < 20; i++)
        {
            step += .05f;
            yield return new WaitForSeconds(.05f);
        }
        SceneManager.LoadScene("Minipuzzle");
    }
    public void OverItem(bool over)
    {
        if (over)
            OnMouseOver?.Invoke();
        else OnMouseExit?.Invoke();
        
    }
    private void OnDestroy()
    {
        SetVariable.StartPuzzleEvent -= StartPuzzle;
    }

}
