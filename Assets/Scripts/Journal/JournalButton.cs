using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JournalButton : MonoBehaviour
{
    [SerializeField] FieldType fieldType;
    [SerializeField] JournalManager journal_Manager;
    [SerializeField] public bool newInfo = false;
    [SerializeField] Sprite open, closed;
    public Sprite Open => open;
    public Sprite Closed => closed;
    [SerializeField] Color currentColor;
    public Color CurrentColor => currentColor;
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
        journal_Manager._lastFieldChanged = fieldType;
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

    public IEnumerator FadeToWhiteColor()
    {
        float i = 0;

        while(i<=1)
        {
            GetComponent<Image>().color = Color.Lerp(journal_Manager.GlowColor, Color.white, i);
            i += 0.1f;
            yield return new WaitForSeconds(.1f);
        }
        GetComponent<Image>().color = Color.white;
        currentColor = Color.white;
        JournalSaveSystem.Instance.newInfoFields[(int)fieldType] = false;
        journal_Manager._currentButtonPressed = this;
        JournalSaveSystem.Instance.lastFieldChanged = fieldType;
        // journal_Manager._lastFieldChanged = fieldType;
        newInfo = false;
        journal_Manager.CheckNewInfo();
    }
    // change color when new info available
    public void ButtonGlow(bool glow)
    {
        if (glow)
        {
            JournalSaveSystem.Instance.newInfoFields[(int)fieldType] = true;
            GetComponent<Image>().color = journal_Manager.GlowColor;
            currentColor = journal_Manager.GlowColor; 
        }
        else
        {
            JournalSaveSystem.Instance.newInfoFields[(int)fieldType] = false;
            if(currentColor == journal_Manager.GlowColor)
            StartCoroutine(FadeToWhiteColor());
            GetComponent<Image>().color = Color.white;
            currentColor = Color.white;
            newInfo = false;
            journal_Manager.CheckNewInfo();
        }
    }
}
