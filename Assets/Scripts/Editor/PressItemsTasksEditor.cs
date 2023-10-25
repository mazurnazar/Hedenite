using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PressItemsTasks))]
public class PressItemsTasksEditor : Editor
{
    private bool pressedButton;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PressItemsTasks pressItemsTasks = (PressItemsTasks)target;

        if (GUILayout.Button("Add new task"))
        {
            NewTask newTask = (NewTask)ScriptableObject.CreateInstance<NewTask>();

            pressItemsTasks.tasks.Add(newTask);
            pressedButton = true;
            AssetDatabase.AddObjectToAsset(newTask, pressItemsTasks);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        for (int i = 0; i < pressItemsTasks.tasks.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Task {i + 1}:");
            pressItemsTasks.tasks[i] = EditorGUILayout.ObjectField($"Task {i + 1}", pressItemsTasks.tasks[i], typeof(NewTask), false) as NewTask;

            pressItemsTasks.tasks[i].description = EditorGUILayout.TextField("Description", pressItemsTasks.tasks[i].description);

            if (pressItemsTasks.tasks[i] != null)
            {
                List<string> items = pressItemsTasks.tasks[i].itemNames;
                int size = Mathf.Max(0, EditorGUILayout.IntField("Size", items.Count));
                while (size > items.Count)
                {
                    items.Add(null);
                }
                while (size < items.Count)
                {
                    items.RemoveAt(items.Count - 1);
                }
                for (int j = 0; j < items.Count; j++)
                {
                    items[j] = EditorGUILayout.TextField("Name " + j, items[j]);
                    //items[j] = EditorGUILayout.ObjectField("Element " + j, items[j], typeof(Item), true) as Item;
                }
                if (GUILayout.Button("Delete"))
                {
                    NewTask taskToDelete = pressItemsTasks.tasks[i];
                    pressItemsTasks.tasks.RemoveAt(i);
                    DestroyImmediate(taskToDelete, true);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    i--; // Decrement the index to account for the removed item
                }
                EditorUtility.SetDirty(pressItemsTasks);
            }

            EditorGUILayout.EndVertical();
        }
     

    }

}
