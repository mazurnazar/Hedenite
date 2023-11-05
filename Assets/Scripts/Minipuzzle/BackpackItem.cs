using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Fungus;
using UnityEngine.UI;
using TMPro;

public class BackpackItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite image;
    [SerializeField] private string description;
    [SerializeField] private Vector2 backpackPlace;
    [SerializeField] private Vector2 tablePos;
    [SerializeField] SpriteRenderer backpack;

    [SerializeField] private bool canPut;
    [SerializeField] PolygonCollider2D objectOverArea;
    [SerializeField] private BackpackItem objectOverItem;
   
    /*private float timedelay = .3f;
    private float currentTime;*/

    private Vector3 initPos;
    private Vector3 currentPos;
    private Vector3 offset;

    [SerializeField] private int objectsOver = 0;

    private const string putTag = "putArea";
    private const string itemTag = "item";
    
    [SerializeField] private float distanceToIdeal = 0.1f;
    //private int rotationAngle = 45;

    private void Start()
    {
        transform.localPosition = tablePos;
    }

    private void OnMouseDown()
    {
        offset = gameObject.transform.position - 
                 Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                           Input.mousePosition.y, 10.0f));
        initPos = gameObject.transform.position;
        GetComponent<SpriteRenderer>().sortingOrder++;
    }


    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().sortingOrder--;
        if ((canPut && objectsOver < 2 && IsItemInsideOutArea())||CheckDistanceToIdeal())
        {
            if (CheckDistanceToIdeal())
                transform.localPosition = backpackPlace;
            else
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(currentPos) + offset;
        }
        else gameObject.transform.position = initPos;

    }
    bool IsItemInsideOutArea()
    {
        // Iterate through all vertices of Object 1's PolygonCollider2D
        PolygonCollider2D item = GetComponent<PolygonCollider2D>();

        bool allPointsInside = true; // Assume all points are inside initially

        // Iterate through all vertices of Object 1's PolygonCollider2D
        foreach (Vector2 vertex in item.points)
        {
            // Convert the vertex position to world space
            Vector2 worldVertex = item.transform.TransformPoint(vertex);
           
            // Check if the vertex is inside Object 2
            if (!objectOverArea.OverlapPoint(worldVertex))
            {
                allPointsInside = false;
                return false;// If any point is outside Object 2, set to false
            }

            // Draw a line to visualize the point being checked
        }

        return allPointsInside; // Return the result
    }
    public bool CheckDistanceToIdeal()
    {
        if (Vector2.Distance(transform.localPosition, backpackPlace) < distanceToIdeal)
        {
            if (objectOverItem == null) return true;
            if (objectOverItem.CheckDistanceToIdeal()) return true;
            else return false;
           // return true;
        }
        return false;
    }

    private void OnMouseDrag()
    {
        currentPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(currentPos) + offset;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOver++;

        if (collision.tag == itemTag)
        {
            canPut = false;
            objectOverItem = collision.GetComponent<BackpackItem>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        // Debug.Log(collision.tag);
        if (collision.tag != putTag)
        {
            canPut = false;
            return;
        }
        if (collision.tag == putTag)
        {
            canPut = true;
            objectOverArea = collision.GetComponent<PolygonCollider2D>();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOver--;

        if (collision.tag == itemTag)
        {
            canPut = false;
            objectOverItem = null;
        }
        //Debug.Log("exit " + gameObject.name +" "+ collision.name);
    }

}
