using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapValidator : MonoBehaviour
{
    public static Vector2 FollowerPos => Instance.follower.Jumper.transform.position;
    public static Action<MapGraph> callback;
    public MapGraph Graph;
    [field: SerializeField] private bool DrawGizmos;
    private PathFindingAgent agent;
    private PathFollower follower;
    public string RejectionReason;
    public static MapValidator Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CreateAgent();
        callback += ReceiveGraph;
    }

    public void ResetValidation()
    {
        ResetReachableBlocks();
        Destroy(agent.gameObject);
        Graph = null;
        CreateAgent();
    }

    private void CreateAgent()
    {
        agent = new GameObject("PathFindingAgent").AddComponent<PathFindingAgent>();
        follower = agent.gameObject.AddComponent<PathFollower>();
        agent.transform.parent = transform.parent;
    }

    private void ReceiveGraph(MapGraph graph)
    {
        RejectionReason = "No path found...";
        if (graph == null)
        {
            Debug.LogWarning("Received null graph. Aborting...");
            GameManager.ChangeGameState(GameState.EditingLevel);
            return;
        }
        Graph = graph;
        Vector2Int start = LevelCreator.LevelData.PlayerPos.Value;
        Vector2Int goal = LevelCreator.LevelData.GoalPos.Value;
        var path = GraphSolver.SolveForPath(Graph, start, goal);
        if (path != null)
        {
            GameManager.ChangeGameState(GameState.StreamerPlaying);
        }
        else
        {
            MarkReachableBlocks();
            GameManager.ChangeGameState(GameState.EditingLevel);
        }
        Debug.Log("Received Graph!");
    }

    private void MarkReachableBlocks()
    {
        if (Graph == null)
            return;
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Stack<Vector2Int> toMark = new Stack<Vector2Int>();
        LevelData levelData = LevelCreator.LevelData;
        Vector2Int current = levelData.PlayerPos.Value + Vector2Int.down;
        toMark.Push(current);
        while (toMark.Count > 0)
        {
            current = toMark.Pop();
            if (visited.Contains(current))
                continue;
            levelData.blocks[current].SetPreviewState(true, true);
            Debug.Log($"Marking {current}");
            visited.Add(current);
            foreach (var edge in Graph.NodeLookupTable[current].JumpEdges)
            {
                if (edge.source == current && !visited.Contains(edge.dest))
                    toMark.Push(edge.dest);
            }
        }
    }
    private void ResetReachableBlocks()
    {
        LevelData levelData = LevelCreator.LevelData;
        foreach (BaseBlock block in levelData.blocks.GetArr())
        {
            if (block != null)
                block.SetPreviewState(false);
        }
    }
    public void FindAndFollowPath()
    {
        Vector2Int start = LevelCreator.LevelData.PlayerPos.Value;
        Vector2Int goal = LevelCreator.LevelData.GoalPos.Value;
        follower.PlayPath(Graph, GraphSolver.SolveForPath(Graph, start, goal));
    }

    public void ValidateAndMapLevelData()
    {
        ResetReachableBlocks();
        if (!LevelCreator.LevelData.HasGoalAndPlayerBlocks())
        {
            Debug.LogWarning("Player or Goal block are missing. Aborting...");
            RejectionReason = "Player or Goal block missing";
            StartCoroutine(delayedReject());
            return;
        }
        ResetValidation();
        MapGraph graph = new MapGraph(LevelCreator.LevelData);
        // agent.transform.parent = transform.parent;
        agent.ExploreLevelData(ReceiveGraph, graph, LevelCreator.LevelData);
    }
    private IEnumerator delayedReject()
    {
        yield return null;
        GameManager.ChangeGameState(GameState.EditingLevel);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        callback -= ReceiveGraph;
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