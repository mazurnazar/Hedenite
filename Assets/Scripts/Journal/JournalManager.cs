using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class JournalManager : MonoBehaviour
{
    public static JournalManager _instance;
    public List<FieldInfo> _fields;
    public List<JournalButton> _buttons;
    public JournalButton _currentButtonPressed;
    public FieldType _lastFieldChanged;
    
    public GameObject JournalButton;
    public GameObject Journal;

    public bool _journalGlowing = false;

    public delegate void OnJournal(bool active);
    public event OnJournal JournalOn;
    public Scene scene;

    [SerializeField] Color glowColor;
    public Color GlowColor => glowColor;
    private void Start()
    {
        //JournalSaveSystem.Instance.LoadInfo();
        _instance = this;

        foreach (FieldInfo item in _fields)
        {
            if (item._fieldType == JournalSaveSystem.Instance.lastFieldChanged)
                _lastFieldChanged = JournalSaveSystem.Instance.lastFieldChanged;
            item.CreateTextFieldEvent();
            item.GetComponent<FieldInfo>().JournalGlow += JournalGlowOn;

        }
        foreach (var item in JournalSaveSystem.Instance.newInfoFields)
        {
            if (item) JournalGlowOn();
        }
        foreach (var item in scene.allItems)
        {
            item.SubscribeToJournalEvent();
        }
        ResetJournal();
        JournalButton.GetComponent<Image>().sprite = JournalSaveSystem.Instance.journalLogo;
        // TurnOff();
    }
    public void CheckNewInfo()
    {
        foreach (var item in _buttons)
        {
            if (item.newInfo) return;
        }
        Debug.Log("no new info");
        JournalGlowOff();
    }
    public void JournalGlowOn()
    {
        if (_journalGlowing) return;
        JournalButton.GetComponent<Image>().color = glowColor;
        _journalGlowing = true;
    }
    public void JournalGlowOff()
    {
        if (!_journalGlowing) return;
        JournalButton.GetComponent<Image>().color = Color.white;
        _journalGlowing = false;
    }
    public void TurnOn()
    {
        ResetJournal();
        JournalButton.SetActive(false);
        Journal.SetActive(true);
        JournalOn?.Invoke(true);

    }
    public void TurnOff()
    {
        JournalButton.SetActive(true);
        Journal.SetActive(false);
        JournalOn?.Invoke(false);
    }
    // open journal on last changed page
    public void ResetJournal()
    {
        for (int i = 0; i < _fields.Count; i++)
        {
            if (_lastFieldChanged == _fields[i]._fieldType)
            {
                _buttons[i].GetComponent<Image>().sprite = _buttons[i].Open;
                _fields[i].gameObject.SetActive(true);
               // _buttons[i].ButtonGlow(false);
               if(_buttons[i].CurrentColor==glowColor)
                StartCoroutine(_buttons[i].FadeToWhiteColor());
            }
            else
            {
                _buttons[i].GetComponent<Image>().sprite = _buttons[i].Closed;
                _fields[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnDestroy()
    {
        //Debug.Log("1 deleted");
        foreach (FieldInfo item in _fields)
        {
            item.Unsubscribe();
        }
    }
}
