using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapValidator))]
public class MapValidatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MapValidator validator = (MapValidator)target;
        if (!Application.isPlaying)
            GUI.enabled = false;
        if (GUILayout.Button("ValidateMap"))
        {
            validator.TestGraph();
        }
        if (GUILayout.Button("PlayPath"))
        {
            validator.FindAndFollowPath();
        }
        GUILayout.Space(10);
        if (validator.Graph?.Nodes != null)
        {
            GUILayout.Label("Graph:");
            GUILayout.Label("Nodecount: " + validator.Graph.Nodes.Count.ToString());
            int edgeCount = 0;
            foreach (var node in validator.Graph.Nodes)
            {
                edgeCount += node.JumpEdges.Count;
            }
            GUILayout.Label($"Edgecount: {edgeCount}");
            string graphstring = "{\n";
            foreach (var node in validator.Graph.Nodes)
            {
                graphstring += "[";
                foreach (var point in node.points)
                {
                    graphstring += $"{point}";
                    if (point != node.points.Last())
                        graphstring += ",";
                }
                graphstring += "]\n";
            }
            graphstring += "}";
            GUILayout.Label(graphstring);
        }
    }
}