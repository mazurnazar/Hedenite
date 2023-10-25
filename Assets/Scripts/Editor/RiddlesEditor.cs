using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Riddles))]
public class RiddlesEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        Riddles riddles = (Riddles)target;

        if (GUILayout.Button("Add riddles"))
        {

            Riddle newRiddle = (Riddle)ScriptableObject.CreateInstance<Riddle>();

            riddles.riddles.Add(newRiddle);
            AssetDatabase.AddObjectToAsset(newRiddle, riddles);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        //  int size = riddles.riddles.Count;

        List<Riddle> items = riddles.riddles;


        for (int i = 0; i < items.Count; i++)
        {
           
            if (items[i] != null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Riddle {i + 1}:");
                items[i].RiddleName = EditorGUILayout.TextField("Riddle ", items[i].RiddleName);
                items[i].rightItem = EditorGUILayout.TextField("Item ", items[i].rightItem);
                items[i].rightAnswer = EditorGUILayout.TextField("Right Answer ", items[i].rightAnswer);
                List<string> wrongAnswers = riddles.riddles[i].wrongAnswers;
                
                int size = Mathf.Max(0, EditorGUILayout.IntField("Size", wrongAnswers.Count));
                
                while (size > wrongAnswers.Count)
                {
                    wrongAnswers.Add(null);
                }
                while (size < wrongAnswers.Count)
                {
                    wrongAnswers.RemoveAt(wrongAnswers.Count - 1);
                }

                EditorGUILayout.LabelField("Wrong answers:");
                EditorGUILayout.BeginVertical("box");
                for (int j = 0; j < wrongAnswers.Count; j++)
                {
                    wrongAnswers[j] = EditorGUILayout.TextField("Answer " + (j+1),wrongAnswers[j]);
                    //items[j] = EditorGUILayout.ObjectField("Element " + j, items[j], typeof(Item), true) as Item;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
            EditorUtility.SetDirty(riddles);
        }
        if (GUILayout.Button("Delete"))
        {
            if (items.Count > 0)
            {
                Riddle riddleToDelete = items[items.Count - 1];
                riddles.riddles.RemoveAt(items.Count - 1);
                DestroyImmediate(riddleToDelete, true);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

    }*/
}
