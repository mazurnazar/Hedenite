using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using TMPro;
public enum FieldType
{
    Log,
    Quest,
    Fact,
    Skill
}
public class FieldInfo : MonoBehaviour
{
    public List<TextMeshProUGUI> _fieldsTMPro;
    public List<string> _text_fields;
    public FieldType _fieldType;

    public JournalButton _journalButton;

    public JournalManager _journal_Manager;

    public delegate void Journal();
    public event Journal JournalGlow;

   // public const string emptyPageText = "It's empty, yet.";
    public void CreateTextFieldEvent()
    {
        SetFields();
        SetVariable.AddTextFieldEvent += CreateTextField;
    }
    void SetFields()
    {
        switch (_fieldType)
        {
            case FieldType.Log:
                SetField(JournalSaveSystem.Instance.logs);
                break;
            case FieldType.Quest:
                SetField(JournalSaveSystem.Instance.quests);
                break;
            case FieldType.Fact:
                SetField(JournalSaveSystem.Instance.facts);
                break;
            case FieldType.Skill:
                SetField(JournalSaveSystem.Instance.skills);
                break;
        }
    }
    public void SetField(List<string> texts)
    {
        TextMeshProUGUI tmProSample = GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI tmProNew = new TextMeshProUGUI();
        int counter = 0;
        //Debug.Log("quests " + texts.Count);
        for (int i = 0; i < texts.Count; i++)
        {
            if (i == 0) tmProNew = tmProSample;
            else
                tmProNew = Instantiate(tmProSample, tmProSample.transform.parent);

            _fieldsTMPro.Add(tmProNew);
            if(texts[i]!=JournalSaveSystem.Instance.emptyPageText)
            _text_fields.Add(texts[i]);
            _fieldsTMPro[counter].text = texts[i];

            if (_fieldType == FieldType.Quest)
            {
                if (JournalSaveSystem.Instance.questComplete.Count > 0)
                {
                    if (JournalSaveSystem.Instance.questComplete.Contains(texts[i]))
                    {
                        Debug.Log(_fieldsTMPro[counter].text);
                        _fieldsTMPro[counter].color = Color.green;
                        _fieldsTMPro[counter].fontStyle = FontStyles.Strikethrough;
                    }
                }
            }

            counter++;

        }
    }
    public void Unsubscribe()
    {
        SetVariable.AddTextFieldEvent -= CreateTextField;
    }
    // create new TextMeshPro in Journal
    public void CreateTextField(string variable, string text)
    {
        if (variable == "QuestDone" && _fieldType == FieldType.Quest) QuestDone(text);
        TextMeshProUGUI tmPro = GetComponentsInChildren<TextMeshProUGUI>()[0];

        if (variable == "Date" && _fieldType == FieldType.Quest)
        {
            CreateField(tmPro, text, true);
        }
        if (variable != _fieldType.ToString()) return;
        if (_text_fields.Count == 0)
        {
            CreateField(tmPro, text, true);
        }
        else
        {
            foreach (var item in _text_fields)
            {
                if (text == item) return;
            }
            TextMeshProUGUI tMPro_New = Instantiate(tmPro, tmPro.transform.parent);
            CreateField(tMPro_New, text, false);
        }
        JournalGlow?.Invoke();
        _journalButton.newInfo = true;
        _journalButton.ButtonGlow(true);
    }
    public void CreateField(TextMeshProUGUI tmPro, string text, bool firstAdd)
    {
        //Debug.Log(text);
        string newText = text.Replace('{', '<').Replace('}', '>');

        _fieldsTMPro.Add(tmPro);
        _text_fields.Add(newText);
      
        tmPro.text = newText;
        _journal_Manager._lastFieldChanged = _fieldType;
        if (firstAdd) ChangeJournalText(newText);
        else
            JournalSaveAdd(newText);

    }
    public void JournalSaveAdd(string text)
    {
        switch (_fieldType)
        {
            case FieldType.Log:
                JournalSaveSystem.Instance.logs.Add(text);
                break;
            case FieldType.Quest:
                JournalSaveSystem.Instance.quests.Add(text);
                break;
            case FieldType.Fact:
                JournalSaveSystem.Instance.facts.Add(text);
                break;
            case FieldType.Skill:
                JournalSaveSystem.Instance.skills.Add(text);
                break;
        }
        JournalSaveSystem.Instance.SaveInfo();
    }
    public void ChangeJournalText(string text)
    {
        switch (_fieldType)
        {
            case FieldType.Log:
                JournalSaveSystem.Instance.logs[0] = text; ;
                break;
            case FieldType.Quest:
                JournalSaveSystem.Instance.quests[0] = text;
                break;
            case FieldType.Fact:
                JournalSaveSystem.Instance.facts[0] = text;
                break;
            case FieldType.Skill:
                JournalSaveSystem.Instance.skills[0] = text; ;
                break;
        }
        JournalSaveSystem.Instance.SaveInfo();
    }
    
    public void QuestDone(string text)
    {
        Debug.Log("quest done " + text);
        string newText = text.Replace('{', '<').Replace('}', '>');
        
        int index = _text_fields.IndexOf(newText);

        _fieldsTMPro[index].color = Color.green;
        _fieldsTMPro[index].fontStyle = FontStyles.Strikethrough;
        JournalSaveSystem.Instance.questComplete.Add(newText);
    }
    private void OnDestroy()
    {
    }
}


