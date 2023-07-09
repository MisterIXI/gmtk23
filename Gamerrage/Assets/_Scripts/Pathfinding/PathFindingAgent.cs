using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFindingAgent : MonoBehaviour
{
    private GameSettings _settings;
    public bool IsSimulating => _graph != null;
    private MapGraph _graph;
    private LevelData _levelData;
    private JumpController _jumper;
    private IEnumerator _iter;
    private float _currentStrength;
    private bool _jumpingLeft;
    private GraphNode _currentNode;
    private Vector2Int _currentCoord;
    private float _holdingStillCounter = 0f;
    private HashSet<CoordPair> coordPairs;
    private void Awake()
    {
        _settings = SettingsHolder.Instance.GameSettings;
    }

    private void FixedUpdate()
    {
        if (!IsSimulating)
            return;
        if (!_jumper.IsMoving)
        {
            _holdingStillCounter += Time.fixedDeltaTime;
            if (_holdingStillCounter >= _settings.TimeToHoldStill)
            {
                GraphNode targetNode = _graph.GetGraphNode(LevelCreator.PosToCoord(_jumper.transform.position + Vector3.down));
                // Debug.Log("Thinking about edgess....");
                if (targetNode != null)
                {
                    // Debug.Log("Adding Edge!");
                    // finish current jump
                    GraphEdge edge = new GraphEdge();
                    edge.source = _currentCoord;
                    edge.dest = LevelCreator.PosToCoord(_jumper.transform.position + Vector3.down);
                    edge.jumpStrength = _currentStrength;
                    edge.isDirLeft = _jumpingLeft;
                    CoordPair pair = new CoordPair();
                    pair.NodeA = edge.source;
                    pair.NodeB = edge.dest;
                    if (coordPairs.Add(pair))
                        _currentNode.JumpEdges.Add(edge);
                }
                _iter.MoveNext();
            }
        }
        else
            _holdingStillCounter = 0;
    }
    public void ExploreLevelData(Action<MapGraph> callback, MapGraph graph, LevelData levelData)
    {
        coordPairs = new HashSet<CoordPair>();
        _graph = graph;
        _levelData = levelData;
        _jumper = Instantiate(_settings.PlayerPrefab);
        _iter = JumpIterator(graph.NodeIterator(), callback);
        _iter.MoveNext();
        _jumper.SetValidator(true);
    }

    private void NextJump(Vector2 position, bool jumpLeft, float strength)
    {
        // Debug.Log($"Next Jump at {position} with {(jumpLeft ? "Left" : "right")} direction and {strength}f strength");
        _jumper.rb.velocity = Vector2.zero;
        _jumper.transform.position = position;
        _jumper.InstantJump(jumpLeft, strength);
    }

    private IEnumerator JumpIterator(IEnumerator<GraphNode> inner_iter, Action<MapGraph> callback)
    {
        // Debug.Log("Jumpiter start");
        Time.timeScale = 100;
        float strengthStep = (_settings.JumpStrengthRange.y - _settings.JumpStrengthRange.x) / _settings.AutoJumpTestSteps;
        while (inner_iter.MoveNext())
        {
            _currentNode = inner_iter.Current;
            foreach (var coord in _currentNode.points)
            {
                _jumpingLeft = true;
                _currentStrength = _settings.JumpStrengthRange.x;
                _currentCoord = coord;
                for (int i = 0; i < _settings.AutoJumpTestSteps; i++)
                {
                    NextJump(LevelCreator.CoordToPos(coord + Vector2Int.up), _jumpingLeft, _currentStrength);
                    yield return null;
                    _currentStrength += strengthStep;
                }
                _jumpingLeft = false;
                _currentStrength = _settings.JumpStrengthRange.x;
                _currentCoord = coord;
                for (int i = 0; i < _settings.AutoJumpTestSteps; i++)
                {
                    NextJump(LevelCreator.CoordToPos(coord + Vector2Int.up), _jumpingLeft, _currentStrength);
                    yield return null;
                    _currentStrength += strengthStep;
                }
            }
        }
        Time.timeScale = 1;
        callback?.Invoke(_graph);
        _graph = null;
        Destroy(_jumper.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (_jumper == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_jumper.transform.position + Vector3.down, 0.5f);
    }
}
struct CoordPair
{
    public Vector2Int NodeA;
    public Vector2Int NodeB;
}