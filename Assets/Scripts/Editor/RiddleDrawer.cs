using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Riddle))]
public class RiddleDrawer :PropertyDrawer
{
    private SerializedProperty riddleNumber;
    private SerializedProperty riddleName;
    private SerializedProperty rightItem;
    private SerializedProperty rightAnswer;
    private SerializedProperty wrongAnswers;

    private int totalLines = 1;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        totalLines = 1;
        
        EditorGUILayout.BeginVertical("box");
        EditorGUI.BeginProperty(position, label, property);
        riddleNumber = property.FindPropertyRelative("riddleNumber");
        riddleName = property.FindPropertyRelative("riddleName");
        rightItem = property.FindPropertyRelative("rightItem");
        rightAnswer = property.FindPropertyRelative("rightAnswer");
        wrongAnswers = property.FindPropertyRelative("wrongAnswers");

        Rect foldOutBox = new Rect(position.min.x, position.min.y,position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        if (property.isExpanded)
        {
            DrawRiddleNumber(position);
            DrawNameProperty(position);
            DrawRightItemProperty(position);
            DrawRightAnswerProperty(position);
            DrawWrongAnswersProperty(position, property, label);
            GetPropertyHeight(property, label);
        }
        

        EditorGUI.EndProperty();
        EditorGUILayout.EndVertical();
    }

    private void DrawRiddleNumber(Rect position)
    {
        Rect drawArea = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight, position.size.x, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(drawArea, riddleNumber, new GUIContent("Riddle"));
        totalLines++;
       
    }
    private void DrawNameProperty(Rect position)
    {
        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight*totalLines;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;
        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, riddleName, new GUIContent("Name"));
        totalLines++;
    }

    private void DrawRightItemProperty(Rect position)
    {
        Rect drawArea = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight*totalLines, position.size.x, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(drawArea, rightItem, new GUIContent("Item"));
        totalLines++;
    }

    private void DrawRightAnswerProperty(Rect position)
    {
        Rect drawArea = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight * totalLines, position.size.x, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.PropertyField(drawArea, rightAnswer, new GUIContent("Right Answer"));
        totalLines += 2;
    }
    private void DrawWrongAnswersProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        int height = 1;
        
        if (wrongAnswers.arraySize>0)
        {
             height = wrongAnswers.arraySize;
             
        }
       
        Rect drawArea = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight * totalLines, position.size.x, EditorGUIUtility.singleLineHeight * height);

        EditorGUI.PropertyField(drawArea, wrongAnswers, new GUIContent("Wrong Answer"));
        if (wrongAnswers.isExpanded) { height = wrongAnswers.arraySize; totalLines += height + 3; GetPropertyHeight(property, label); }
        totalLines += 1;
        
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int total = 1;
        if (property.isExpanded) total += 15;
        return (EditorGUIUtility.singleLineHeight*total);
    }

}
