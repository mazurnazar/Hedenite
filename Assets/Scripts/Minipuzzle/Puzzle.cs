using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] allItems;
    [SerializeField] private int allRightPlaces;
    [SerializeField] private int currentFilledPlaces;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private Material outlineMaterial;

    void Start()
    {
        Say.DialogueStarted += Say_DialogueStarted;
        Say.DialogueStopped += Say_DialogueStopped;
    }

    private void Say_DialogueStopped()
    {

        Debug.Log("dialog stop");
        foreach (var item in allItems)
        {
            item.GetComponent<Collider2D>().enabled = true;
        }
        DrawOutLine();
    }

    private void DrawOutLine()
    {
        lineRenderer.material = outlineMaterial;
        float zPos = 0;
        Vector2[] pColiderPos = polygonCollider.points;
        for (int i = 0; i < pColiderPos.Length; i++)
        {
            pColiderPos[i] = polygonCollider.transform.TransformPoint(pColiderPos[i]);
        }
        lineRenderer.positionCount = pColiderPos.Length + 1;
        for (int i = 0; i < pColiderPos.Length; i++)
        {
            //6. Draw the  line
            Vector3 finalLine = pColiderPos[i];
            finalLine.z = zPos;
            lineRenderer.SetPosition(i, finalLine);

            //7. Check if this is the last loop. Now Close the Line drawn
            if (i == (pColiderPos.Length - 1))
            {
                finalLine = pColiderPos[0];
                finalLine.z = zPos;
                lineRenderer.SetPosition(pColiderPos.Length, finalLine);
            }
        }
    }

    private void Say_DialogueStarted()
    {
        Debug.Log("dialog start");
        foreach (var item in allItems)
        {
            item.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnDestroy()
    {

        Say.DialogueStarted -= Say_DialogueStarted;
        Say.DialogueStopped -= Say_DialogueStopped;
    }
}
