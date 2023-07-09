using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor
{
    Vector2Int dimensions;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        dimensions = EditorGUILayout.Vector2IntField("Dimensions", dimensions);
        var lc = (LevelCreator)target;
        if(!Application.isPlaying)
            GUI.enabled = false;
        if (GUILayout.Button("Create Base"))
        {
            lc.CreateData(dimensions);
        }
    }
}