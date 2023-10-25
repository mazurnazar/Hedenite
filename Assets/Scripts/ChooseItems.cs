using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
public class ChooseItems : MonoBehaviour
{
    public static ChooseItems Instance;
    [SerializeField] Button[] buttons;
    [SerializeField] private Button chosenItem;
    [SerializeField] public bool canChoose;
    [SerializeField] GameObject table;
    [SerializeField] Flowchart flowchart;
    [SerializeField] SayDialog sayDialog;
    [SerializeField] Character character;
    [SerializeField] bool swing = true;

    private Vector2 center;
    private void Start()
    {
        Instance = this;
        canChoose = true;
    }
    
    public void SaveChosen(Button button)
    {
        chosenItem = button;
        button.GetComponent<BackpackButton>().SayDialogBackpack(flowchart, sayDialog, character);
        table.GetComponent<Image>().enabled = false;
        //button.interactable = false;
        foreach (var item in buttons)
        {
            if (item == chosenItem) continue;
            item.gameObject.SetActive(false);
        }
        StartCoroutine(MoveToCenter());
        canChoose = false;
       
    }
    public IEnumerator MoveToCenter()
    {
        Vector2 currentPos = chosenItem.transform.position;
        Vector2 offset = new Vector2(0, 150);
        center = new Vector3(Screen.width / 2, Screen.height / 2, 1);
        center += offset;
        Vector3 currentScale = chosenItem.transform.localScale;
        Vector3 nextScale = currentScale * 1.5f;
        float step = 0;
        for (int i = 0; i < 21; i++)
        {
            chosenItem.transform.position = Vector3.Lerp(currentPos, center, step);
            chosenItem.transform.localScale = Vector3.Lerp(currentScale, nextScale, step);
            step += .05f;
            yield return new WaitForSeconds(.05f);
        }
    }
}
