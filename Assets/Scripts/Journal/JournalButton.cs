using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JournalButton : MonoBehaviour
{
    [SerializeField] FieldType fieldType;
    [SerializeField] JournalManager journal_Manager;
    [SerializeField] public bool newInfo = false;
    [SerializeField] int buttonNumber;
     public Sprite open, closed;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeButtonSprite);
        GetComponent<Button>().onClick.AddListener(ShowField);
        ButtonGlow(JournalSaveSystem.Instance.newInfoFields[(int)fieldType]);
    }

    // show info in a page of journal on click
    public void ShowField()
    {
        foreach (var item in journal_Manager._fields)
        {
            if (item._fieldType == fieldType) item.gameObject.SetActive(true);
            else item.gameObject.SetActive(false);
        }
        newInfo = false;
        ButtonGlow(false);
        journal_Manager.CheckNewInfo();
    }

    // change sprite from open to closed 
    public void ChangeButtonSprite()
    {
        journal_Manager._currentButtonPressed.GetComponent<Image>().sprite =
            journal_Manager._currentButtonPressed.GetComponent<JournalButton>().closed;
        GetComponent<Image>().sprite = open;
        journal_Manager._currentButtonPressed = this;
    }
    // change color when new info available
    public void ButtonGlow(bool glow)
    {
        if (glow)
        {
            JournalSaveSystem.Instance.newInfoFields[(int)fieldType] = true;
            GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            JournalSaveSystem.Instance.newInfoFields[(int)fieldType] = false;
            GetComponent<Image>().color = Color.white;
        }
    }
}
