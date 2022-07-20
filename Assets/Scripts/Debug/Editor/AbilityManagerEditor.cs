using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AbilityController))]
public class AbilityManagerEditor : Editor
{

    AbilityController manager;
    private void OnEnable()
    {
        manager = target as AbilityController;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
    }

    private void OnDisable()
    {

    }
}
