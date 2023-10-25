using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Scene : MonoBehaviour
{
    public new string name;
    public bool moveRight;
    public bool moveLeft;

    public int currentStage = 1;
    public GameObject[] directions;

    [SerializeField] List<Item> neededItemsToContinue;
    [SerializeField] public List<Item> allItems;

    [SerializeField] GameObject [] Exites;
    public AudioMixerGroup mixerGroup;
    //[SerializeField] public PressItemsTasks pressItemsTasks;

    // check if all items in scene is clicked for exiting the room
    private void Start()
    {
        foreach (var item in neededItemsToContinue)
        {
            item.OnPress += CheckItems;
        }
    }
    public void CheckItems()
    { 
        foreach (var item in neededItemsToContinue)
        {
            if (!item.Clicked) return;
            else item.OnPress -= CheckItems;
        }
        //Debug.Log("all items clicked");
        foreach (var item in Exites)
        {
            item.GetComponent<Item>().Interactable = true;
            item.GetComponent<BoxCollider2D>().enabled = true;

        }
        
    }

    private void OnDestroy()
    {
        JournalSaveSystem.Instance.SaveInfo();
    }

}
