using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tasks")]
public class PressItemsTasks :ScriptableObject
{
    public List<NewTask> tasks = new List<NewTask>();
}
