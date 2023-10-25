using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Fungus;
public class MovingScene : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] new string name;
    [SerializeField] bool moveLeft;
    [SerializeField] bool moveRight;
    [SerializeField] ScenesManager sceneManager;

    [SerializeField] Button move;
    
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject[] directions;
    private bool canMove = true;
    [SerializeField] private JournalManager journal_Manager;
    [SerializeField] FindItems findItems;
    
    public void SetMove()
    {
        CheckDirection();
    }
    // when pointer enters this object activate corresponding arrow
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!canMove) return;
        CheckDirection();
        if ((name == "Left" && moveLeft) || (name == "Right" && moveRight))
        move.gameObject.SetActive(true);
    }

    // when pointer exits this object deactivate corresponding arrow
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!canMove) return;
        CheckDirection();
        if ((name == "Left" && moveLeft) || (name == "Right" && moveRight))
            move.gameObject.SetActive(false);
    }
   
    public void ButtonClickedMove(string direction)
    {
        move.gameObject.SetActive(false);
        if(direction=="left")
        sceneManager.CurrentScene.currentStage--;
        else if (direction == "right")
            sceneManager.CurrentScene.currentStage++;
        StartCoroutine(MoveTo(directions[sceneManager.CurrentScene.currentStage]));
    }
    
    public void CheckDirection()
    {
        directions = sceneManager.CurrentScene.directions;
        switch(sceneManager.CurrentScene.currentStage)
        {
            case 1:
                moveLeft = sceneManager.CurrentScene.moveLeft;
                moveRight = sceneManager.CurrentScene.moveRight;
                break;
            case 0:
                moveLeft = false;
                moveRight = true;
                break;
            case 2:
                moveLeft = true;
                moveRight = false;
                break;

        }
    }
    void Start()
    {
        SetMove();
        sceneManager.MoveToScene += SetMove;
        Say.DialogueStarted += DialogStart;
        Say.DialogueStopped += DialogStop;
        if (findItems)
        {
            findItems.OnMouseOver += DialogStart;
            findItems.OnMouseExit += DialogStop;
        }
        journal_Manager.JournalOn += JournalOn;

    }
    public void JournalOn(bool active)
    {
        canMove = !active;
    }

    public void DialogStart() 
    {
        move.gameObject.SetActive(false);
        canMove = false; 
    }
    public void DialogStop() 
    {
       // move.gameObject.SetActive(true);
        canMove = true; 
    }
    // move camera to some point
    public IEnumerator MoveTo(GameObject moveTo)
    {
        float step = 0;
        Vector3 startPos = Camera.transform.position;
        Vector3 endPos = new Vector3(moveTo.transform.position.x, 
                                     moveTo.transform.position.y, 
                                     Camera.transform.position.z);
       
        bool can = true;
        while (can)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, endPos, JournalSaveSystem.Instance.cameraSpeed*Time.deltaTime);
            step += 0.05f;
            yield return new WaitForEndOfFrame();
            if (Vector2.Distance(Camera.transform.position,endPos)<1f) can = false;
        }

    }

    private void OnDestroy()
    {
        Say.DialogueStarted -= DialogStart;
        Say.DialogueStopped -= DialogStop;
        journal_Manager.JournalOn -= JournalOn;

        if (findItems)
        {
            findItems.OnMouseOver -= DialogStart;
            findItems.OnMouseExit -= DialogStop;
        }
    }
}
