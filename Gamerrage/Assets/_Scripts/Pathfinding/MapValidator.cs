using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapValidator : MonoBehaviour
{
    public static Action<MapGraph> callback;
    public MapGraph Graph;
    [field: SerializeField] private bool DrawGizmos;
    private PathFindingAgent agent;
    private PathFollower follower;
    private void Awake()
    {
        agent = new GameObject("PathFindingAgent").AddComponent<PathFindingAgent>();
        follower = agent.gameObject.AddComponent<PathFollower>();
    }
    public void TestGraph()
    {
        callback += ReceiveGraph;
        Graph = ValidateAndMapLevelData(callback, LevelCreator.LevelData);

    }

    private void ReceiveGraph(MapGraph graph)
    {
        Graph = graph;
        Debug.Log("Received Graph!");
        callback -= ReceiveGraph;
    }
    public void FindAndFollowPath()
    {
        Vector2Int start = LevelCreator.LevelData.PlayerPos.Value;
        Vector2Int goal = LevelCreator.LevelData.GoalPos.Value;
        follower.PlayPath(Graph, GraphSolver.SolveForPath(Graph, start, goal));
    }

    public MapGraph ValidateAndMapLevelData(Action<MapGraph> callback, LevelData levelData)
    {
        if (!levelData.HasGoalAndPlayerBlocks())
        {
            Debug.LogWarning("Player or Goal block are missing. Aborting...");
            return null;
        }
        MapGraph graph = new MapGraph(levelData);
        // agent.transform.parent = transform.parent;
        agent.ExploreLevelData(callback, graph, LevelCreator.LevelData);
        return graph;
    }


    [field: SerializeField] private int GizmoPaths;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        if (!DrawGizmos)
            return;
        if (Graph != null)
        {
            // Gizmos.color = Color.green;
            // foreach (var node in Graph.Nodes)
            // {
            //     Vector3 start = LevelCreator.CoordToPos(node.points[0]);
            //     Vector3 end = LevelCreator.CoordToPos(node.points.Last());
            //     float x_dist = end.x - start.x;
            //     Gizmos.DrawCube(new(start.x + x_dist / 2, start.y, -0.25f), new(x_dist + 0.8f, 0.9f, 0.5f));
            // }
            Gizmos.color = Color.blue;
            // int nodeNum = 0;
            foreach (var node in Graph.Nodes)
            {

                // if (nodeNum++ == GizmoPaths)
                // {
                foreach (var edge in node.JumpEdges)
                {
                    Vector3 source = LevelCreator.CoordToPos(edge.source);
                    Vector3 dest = LevelCreator.CoordToPos(edge.dest);
                    Gizmos.DrawLine(source, dest);
                }
                // break;
                // }
            }
        }

    }
}