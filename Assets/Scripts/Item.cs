using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Fungus;
using UnityEngine.UI;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite biggerImage;
    [SerializeField] private string description;
    [SerializeField] private Scene scene;

    [SerializeField] private bool movetoInventory = true;
    [SerializeField] private bool neededForInventory;
    [SerializeField] private bool clicked;
    [SerializeField] private bool interactable;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Item[] aditionalItems;
    [SerializeField] private Vector3 center;
    //[SerializeField] private TextMeshProUGUI showText;
    //[SerializeField] private string exitText;
    [SerializeField] Flowchart flowchart;
    [SerializeField] Camera camera;
    [SerializeField] FindItems findItems;
    [SerializeField] Vector3 offset = new Vector3(0, 3, 0);
    [SerializeField] Vector3 swingOffset = new Vector3(0, 1, 0);

    

    public bool Interactable { get => interactable; set => interactable = value; }
    public string ItemName { get => itemName; }
    public Sprite Image { get => biggerImage; }
    public string Description { get => description; }

    public bool MovetoInventory { get => movetoInventory; set => movetoInventory = value; }
    public bool NeededForInventory { get => neededForInventory; }
    public bool Clicked { get => clicked; }

    public Item[] AditionalItems => aditionalItems;
    public delegate void Press();
    public event Press OnPress;
    private void Start()
    {
       
        Say.DialogueStarted += DialogueStart;
        Say.DialogueStopped += DialogueStop;

    }
    public void SubscribeToJournalEvent()
    {
        JournalManager._instance.JournalOn += ActivateItem;
    }
    public void ActivateItem(bool active)
    {
        if (interactable)
        GetComponent<Collider2D>().enabled =!active;

    }
    public void DialogueStart()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    public void DialogueStop()
    {

        if (interactable)
            GetComponent<Collider2D>().enabled = true;

    }

    private void OnMouseDown()
    {
        if (!neededForInventory)  return;
        if (movetoInventory)
        {
            FindItems.instance.CheckRightItem(this);
        }
    }
    
    private void OnMouseUp()
    {
        Cursor.SetCursor(null, Vector2.zero,CursorMode.Auto);
        clicked = true;
        if (scene != null)
        {
            OnPress?.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        if(findItems)
        findItems.OverItem(true);
    }
    private void OnMouseExit()
    {
        if (findItems)
            findItems.OverItem(false);
    }
    public IEnumerator MoveToCenter()
    {

        Vector3 currentPos = transform.position;

        center = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 1));
        center += offset;
        if (GetComponent<Parallax>())
            GetComponent<Parallax>().enabled = false;

        Vector3 currentScale = transform.localScale;
        Vector3 nextScale = currentScale * 3;
        if (Image != null)
            GetComponent<SpriteRenderer>().sprite = Image;
        float step = 0;
        for (int i = 0; i < 21; i++)
        {
            transform.position = Vector3.Lerp(currentPos, center, step);
            transform.localScale = Vector3.Lerp(currentScale, nextScale, step);
            step += .05f;
            yield return new WaitForSeconds(.05f);
        }
        StartCoroutine(Swing(center));

    }

    public IEnumerator Swing(Vector3 center)
    {
        Vector3 up = center + swingOffset;
        Vector3 down = center - swingOffset;
        Vector3 currentPos = transform.position;
        Vector3 direction = up;

        while (findItems.MoveToInventory)
        {
            float step = 0;
            for (int i = 0; i < 20; i++)
            {
                transform.position = Vector3.Lerp(currentPos, direction, step);
                if (!findItems.MoveToInventory) break;
                step += .05f;
                yield return new WaitForSeconds(.05f);
            }
            currentPos =transform.position;
            direction = Vector2.Distance(currentPos, up) < 1 ? down : up;
        }
    }
    private void OnDestroy()
    {
        Say.DialogueStarted -= DialogueStart;
        Say.DialogueStopped -= DialogueStop;
        JournalManager._instance.JournalOn -= ActivateItem;

    }
}
