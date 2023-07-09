using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public List<Vector2Int> points = new List<Vector2Int>();
    public List<GraphEdge> JumpEdges = new List<GraphEdge>();
    public List<GraphEdge> WalkEdges = new List<GraphEdge>();
}