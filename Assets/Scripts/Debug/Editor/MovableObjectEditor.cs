using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovableObject))]
public class MovableObjectEditor : Editor
{
    private bool shouldLoop = false;
    private MovableObject owner;
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneUpdate;
        owner = target as MovableObject;
    }

    private void OnSceneUpdate(SceneView obj)
    {
        if (owner.path.Length <= 0) return;
        for (int i = 0; i < owner.path.Length; i++)
        {
            int nextI = (i + 1) % owner.path.Length;

            Vector3 p1 = Application.isPlaying ? owner.path[i] /*+ owner.positionAtStart*/ : owner.path[i] + owner.transform.position;

            if (shouldLoop || i + 1 < owner.path.Length)
            {
                Vector3 p2 = Application.isPlaying ? owner.path[nextI] /*+ owner.positionAtStart*/ : owner.path[nextI] + owner.transform.position;
                Handles.color = Color.red;
                Handles.DrawDottedLine(p1, p2, 0.3f);
            }

            Handles.color = Color.blue;
            Handles.Label(p1, new GUIContent { text = $"Pos <{i}>" });


            if (Application.isPlaying) continue;
            owner.path[i] = Handles.PositionHandle(owner.path[i] + owner.transform.position, Quaternion.identity) - owner.transform.position;

        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        shouldLoop = GUILayout.Toggle(shouldLoop, new GUIContent { text = "Loop", tooltip = "Make the path loop!" }, EditorStyles.toolbarButton);

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneUpdate;
    }
}
