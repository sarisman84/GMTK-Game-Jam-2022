using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(AbilityManager))]
public class AbilityManagerEditor : Editor
{

    AbilityManager manager;
    private void OnEnable()
    {
        manager = target as AbilityManager;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnSceneGUI(SceneView obj)
    {
        for (int side = 0; side < 4; side++)
        {
            if (manager.listOfAbilities.Count > side)
            {
                Vector3 normal = manager.GetDiceSide(side);
                Vector3 offset = (manager.transform.position + normal);
                Handles.Label(offset, new GUIContent(text: $"S[{side}]<{manager.listOfAbilities[side]}"));
            }

        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
