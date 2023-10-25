using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject itemsList;
    [SerializeField] private Text itemsFoundText;
    [SerializeField] private FindItems findItems;
    [SerializeField] private GameObject journalButton;

    [SerializeField] private GameObject collectedThings;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private Image leftNumber;
    
    private void Awake()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
    private void Start()
    {
        findItems.Collect += ChangeCollectedText;
        findItems.PuzzleStart += PuzzleStart;
    }
    public void ChangeCollectedText(int number, int total)
    {
        if (number > 9)
        {
            leftNumber.rectTransform.sizeDelta = new Vector2(100, leftNumber.rectTransform.sizeDelta.y);
            leftNumber.rectTransform.anchoredPosition = new Vector2(-100, 0);
        }
        leftNumber.sprite = numbers[number];
       // itemsFoundText.text = number + "/" + total;
    }
    public void ActivateList()
    {
        bool activate = itemsList.activeSelf ? false : true;
        itemsList.SetActive(activate);
    }
    public void PuzzleStart(bool activate)
    {
        ActivateCollected(activate);
        ActivateJournal(activate);
    }
    public void ActivateJournal(bool activate)
    {
        journalButton.SetActive(activate);
    }
   
    public void ActivateCollected(bool activate)
    {
        collectedThings.SetActive(activate);
    }
    private void OnDestroy()
    {

        findItems.Collect -= ChangeCollectedText;
        findItems.PuzzleStart -= PuzzleStart;
    }
}
