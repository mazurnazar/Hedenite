using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Press Item Task")]
public class NewTask : ScriptableObject
{
    public List<string> itemNames = new List<string>();
    public string description;
    public bool completed;
}
