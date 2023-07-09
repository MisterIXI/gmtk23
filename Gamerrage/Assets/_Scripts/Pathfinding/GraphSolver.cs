using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class GraphSolver
{
    public static List<GraphEdge> SolveForPath(MapGraph graph, Vector2Int start, Vector2Int goal)
    {
        PriorityQueue<Vector2Int> openSetQueue = new PriorityQueue<Vector2Int>();
        HashSet<Vector2Int> openSet = new HashSet<Vector2Int>();
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, GraphEdge> cameFrom = new Dictionary<Vector2Int, GraphEdge>();
        Dictionary<Vector2Int, int> gcost = new Dictionary<Vector2Int, int>();
        Dictionary<Vector2Int, List<GraphEdge>> incomingEdgeDict = new Dictionary<Vector2Int, List<GraphEdge>>();
        // discover all relevant edges
        // this must be done to discover reverse
        // Debug.Log("Mapping Edges");
        foreach (var node in graph.Nodes)
        {
            foreach (var edge in node.JumpEdges)
            {
                if (!incomingEdgeDict.ContainsKey(edge.dest))
                    incomingEdgeDict.Add(edge.dest, new List<GraphEdge>());
                incomingEdgeDict[edge.dest].Add(edge);
            }
        }
        start += Vector2Int.down;
        goal += Vector2Int.down;
        bool goalFound = false;
        openSet.Add(start);
        gcost[start] = 0;
        openSetQueue.Enqueue(start, NodeDist(start, goal));
        Vector2Int current;
        // Debug.Log("Beginning Search!");
        while (openSetQueue.Count > 0)
        {
            current = openSetQueue.Dequeue();
            openSet.Remove(current);
            if (current == goal)
            {
                // goal found
                goalFound = true;
                Debug.Log("Goal Found!");
                break;
            }
            closedSet.Add(current);
            foreach (var edge in GetEdges(graph, current))
            {
                Vector2Int neighbour = edge.dest;
                if (closedSet.Contains(neighbour))
                    continue;
                int GCostNeighbour = gcost[current] + 1;
                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                    openSetQueue.Enqueue(neighbour, GCostNeighbour + NodeDist(neighbour, goal));
                }
                else if (gcost[neighbour] <= GCostNeighbour)
                    continue;

                cameFrom[neighbour] = edge;
                gcost[neighbour] = GCostNeighbour;
            }
        }
        List<GraphEdge> resultPath = new List<GraphEdge>();
        if (!goalFound)
        {
            Debug.LogWarning("No Path found...");
            return resultPath;
        }
        // try to reconstruct path
        current = goal;
        while (current != start)
        {
            resultPath.Add(cameFrom[current]);
            current = cameFrom[current].source;
        }
        resultPath.Reverse();
        // Debug.Log("Sending Path!");
        return resultPath;
    }

    private static List<GraphEdge> GetEdges(MapGraph graph, Vector2Int coord)
    {
        List<GraphEdge> edges = new List<GraphEdge>();
        GraphNode node = graph.NodeLookupTable[coord];
        foreach (var edge in node.JumpEdges)
        {
            if (edge.source == coord)
                edges.Add(edge);
        }
        return edges;
    }
    // private static float CalcNodeDist(Vector2Int nodeA, Vector2Int nodeB)
    // {
    //     Vector3 nodeA3 = LevelCreator.CoordToPos(nodeA);
    //     Vector3 nodeB3 = LevelCreator.CoordToPos(nodeB);
    //     return Vector3.Distance(nodeA3, nodeB3);
    // }
    private static int NodeDist(Vector2Int nodeA, Vector2Int nodeB) => (int)Vector2Int.Distance(nodeA, nodeB);
    private static float CalcNodeDist(Vector2Int nodeA, Vector2Int nodeB)
    {
        Vector3 nodeA3 = LevelCreator.GraphCoordToPos(nodeA);
        Vector3 nodeB3 = LevelCreator.GraphCoordToPos(nodeB);
        return Vector3.Distance(nodeA3, nodeB3);
    }
}

