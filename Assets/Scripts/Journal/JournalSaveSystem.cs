using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
public class JournalSaveSystem : MonoBehaviour
{
    public static JournalSaveSystem Instance;

    public string emptyPageText;
    public List<string> logs;
    public List<string> quests;
    public List<string> facts;
    public List<string> skills;
    public Sprite journalLogo;
    public FieldType lastFieldChanged;

    public List<string> questComplete;
    public bool [] newInfoFields;

    public delegate void StopStart();
    public event StopStart stopStart;
    public delegate void GameOver(GameObject gameObject);
    public event GameOver GameOverCheck;
    public float cameraSpeed;
    private const string filename = "jornalsave.json";
    // Start is called before the first frame update

    public void Awake()
    {

        FirstSave();

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //FirstSave();
        LoadInfo();
    }
    public void FirstSave()
    {
        logs.Add(emptyPageText);
        quests.Add(emptyPageText);
        facts.Add(emptyPageText);
        skills.Add(emptyPageText);

        newInfoFields = new bool[4] { false, false, false, false };

        SaveInfo();
        logs = new List<string>();
        quests = new List<string>();
        facts = new List<string>();
        skills = new List<string>();
        
        questComplete = new List<string>();
       
    }

    public void SaveInfo()
    {
        SaveData data = new SaveData();
        for (int i = 0; i < logs.Count; i++)
        {
            data.logs.Add(logs[i]);
        }
        for (int i = 0; i < quests.Count; i++)
        {
            data.quests.Add(quests[i]);
        }
        for (int i = 0; i < facts.Count; i++)
        {
            data.facts.Add(facts[i]);
        }
        for (int i = 0; i < skills.Count; i++)
        {
            data.skills.Add(skills[i]);
        }
        data.lastFieldChanged = lastFieldChanged;

        data.questsComplete.Clear();
        for (int i = 0; i < questComplete.Count; i++)
        {
            data.questsComplete.Add(questComplete[i]);
        }

        for (int i = 0; i < newInfoFields.Length; i++)
        {
            data.newInfoFields[i] = newInfoFields[i];
        }
        data.journalLogo = journalLogo;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/" + filename, json);
        //Debug.Log(Application.persistentDataPath + "/savefile.json");
    }

    public void LoadInfo()
    {
        string path = Application.persistentDataPath + "/" + filename;
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            for (int i = 0; i < data.logs.Count; i++)
            {
                logs.Add(data.logs[i]);

            }
            for (int i = 0; i < data.quests.Count; i++)
            {
                quests.Add(data.quests[i]);
            }
            for (int i = 0; i < data.facts.Count; i++)
            {
                facts.Add(data.facts[i]);
            }

            for (int i = 0; i < data.skills.Count; i++)
            {
                skills.Add(data.skills[i]);
            }
            for (int i = 0; i < data.newInfoFields.Length; i++)
            {
                newInfoFields[i] = data.newInfoFields[i];
            }
            for (int i = 0; i < data.questsComplete.Count; i++)
            {
                questComplete.Add(data.questsComplete[i]);
            }

            lastFieldChanged = data.lastFieldChanged;
            journalLogo = data.journalLogo;
        }
    }
    [System.Serializable]
    class SaveData
    {
        public List<string> logs = new List<string>();
        public List<string> quests = new List<string>();
        public List<string> facts = new List<string>();
        public List<string> skills = new List<string>();

        public List<string> questsComplete = new List<string>();
        public bool[] newInfoFields = new bool[4];

        public FieldType lastFieldChanged;
        public Sprite journalLogo;
    }

}
