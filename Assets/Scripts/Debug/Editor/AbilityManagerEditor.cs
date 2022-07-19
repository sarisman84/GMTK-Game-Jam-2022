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
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnSceneGUI(SceneView obj)
    {
        for (int side = 0; side < 4; side++)
        {
            if (manager.abilities.Count > side)
            {
                //Vector3 normal = manager.GetDiceSide(side);
                //Vector3 offset = (manager.transform.position + normal);
                //Handles.Label(offset, new GUIContent(text: $"S[{side}]<{manager.listOfAbilities[side].abilityLabel}>"));
            }

        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
