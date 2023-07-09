using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGraph
{
    public Dictionary<Vector2Int, GraphNode> NodeLookupTable;
    public List<GraphNode> Nodes;
    private LevelData _levelData;
    public MapGraph(LevelData levelData)
    {
        Nodes = new List<GraphNode>();
        NodeLookupTable = new Dictionary<Vector2Int, GraphNode>();
        this._levelData = levelData;
        BuildMapGraph();
    }

    private void BuildMapGraph()
    {
        GraphNode currentNode = null;
        for (int y = 0; y < _levelData.sizeY - 1; y++)
        {
            // starts at 0 to capture the ground floor
            for (int x = 1; x < _levelData.sizeX - 1; x++)
            {
                // check if valid tile
                if (_levelData.IsAboveFree(new(x, y)) && BlockInfo.IsGroundBlock(_levelData[x, y]))
                {
                    // create node if necessary
                    if (currentNode == null)
                    {
                        currentNode = new GraphNode();
                        Nodes.Add(currentNode);
                    }
                    currentNode.points.Add(new(x, y));
                    NodeLookupTable.Add(new(x, y), currentNode);
                }
                else
                    currentNode = null;
            }
            currentNode = null;
        }
        if (currentNode != null)
            Nodes.Add(currentNode);
    }
    public bool IsGraphNode(Vector2Int coord) => GetGraphNode(coord) != null;
    public GraphNode GetGraphNode(Vector2Int coord)
    {
        foreach (var node in Nodes)
        {
            if (node.points.Contains(coord))
            {
                return node;
            }
        }
        return null;
    }

    public IEnumerator<GraphNode> NodeIterator()
    {
        foreach (var node in Nodes)
        {
            yield return node;
        }
    }
}