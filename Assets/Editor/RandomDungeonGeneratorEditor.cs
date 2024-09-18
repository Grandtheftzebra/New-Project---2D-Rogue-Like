using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    private AbstractDungeonGenerator _generator;

    private void Awake()
    {
        //target is a property provided by the base Editor class.
        //It automatically references the object that the editor is currently inspecting or acting upon.
        //In this context, since RandomDungeonGeneratorEditor is tied to AbstractDungeonGenerator,
        //target will always be an instance of AbstractDungeonGenerator or one of its derived classes.
        _generator = target as AbstractDungeonGenerator; 
        
        if (target == null)
            Debug.Log($"target is {target} in Script: RandomDungeonGeneratorEditor! Check the script.");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Create Dungeon")) // Creates button that when it's clicked performs the code below.
            _generator.GenerateDungeon();
    }
}
