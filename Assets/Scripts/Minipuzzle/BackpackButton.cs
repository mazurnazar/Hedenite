using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class BackpackButton : MonoBehaviour
{
    [SerializeField] string description;
    [SerializeField] Vector2 inventory;
    private void Start()
    {
        Say.DialogueStopped += StartPuzzle;
    }

    public void SayDialogBackpack(Flowchart flowchart, SayDialog sayDialog, Character character)
    {
        Say say = new Say();
        say.SayInfo(flowchart, description, sayDialog, character);
        
    }
    public void StartPuzzle()
    {
        StartCoroutine(MoveToInventory());
        SendMessage sendMessage = new SendMessage();
        sendMessage.ReseiveMessage("Puzzle");
        Say.DialogueStopped -= StartPuzzle;
    }
    private void OnDestroy()
    {
        Say.DialogueStopped -= StartPuzzle;
    }

    public IEnumerator MoveToInventory()
    {
        Vector2 currentPos = transform.localPosition;
        Vector2 nextPos = inventory;
        float step = 0;
        for (int i = 0; i < 21; i++)
        {
            transform.localPosition = Vector3.Lerp(currentPos, nextPos, step);
            step += .05f;
            yield return new WaitForSeconds(.05f);
        }
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Say.DialogueStopped -= StartPuzzle;
    }
}
