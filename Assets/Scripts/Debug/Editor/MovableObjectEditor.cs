using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovableObject))]
public class MovableObjectEditor : Editor
{
    private MovableObject owner;
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneUpdate;
        owner = target as MovableObject;
    }

    private void OnSceneUpdate(SceneView obj)
    {
        if (owner.path == null || owner.path.Length <= 0) return;
        for (int i = 0; i < owner.path.Length; i++)
        {
            int nextI = (i + 1) % owner.path.Length;

            Vector3 p1 = Application.isPlaying ? owner.path[i] + owner.positionOnStart : owner.path[i] + owner.transform.position;

            if (owner.looping || i + 1 < owner.path.Length)
            {
                Vector3 p2 = Application.isPlaying ? owner.path[nextI] + owner.positionOnStart : owner.path[nextI] + owner.transform.position;
                Handles.color = Color.red;
                Handles.DrawDottedLine(p1, p2, 0.3f);
            }

            Handles.color = Color.blue;
            Handles.Label(p1, new GUIContent { text = $"Pos <{i}>" });



            owner.path[i] = Handles.PositionHandle(Application.isPlaying ? owner.path[i] + owner.positionOnStart : owner.path[i] + owner.transform.position, Quaternion.identity) - (Application.isPlaying ? owner.positionOnStart : owner.transform.position);

        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        owner.looping = GUILayout.Toggle(owner.looping, new GUIContent { text = "Loop", tooltip = "Make the path loop!" }, EditorStyles.toolbarButton);

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneUpdate;
    }
}
