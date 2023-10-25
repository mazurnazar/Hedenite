using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Riddles")]
public class Riddles : ScriptableObject
{
    public List<Riddle> riddles = new List<Riddle>();
}


[System.Serializable]
public class Riddle
{
    public string riddleNumber;
    public string riddleName;
    public string rightItem;
    public string rightAnswer;
    public List<string> wrongAnswers = new List<string>();
   
}

