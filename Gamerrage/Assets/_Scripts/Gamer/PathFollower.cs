using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFollower : MonoBehaviour
{

    public JumpController Jumper { get; private set; }
    private MapGraph _graph;
    private List<GraphEdge> _path;
    private GameSettings _settings;
    private float _restingTime;
    private Vector2Int targetCoord;
    private FollowState _state;
    private Vector2 _waddleTarget;
    private bool _startedPlaying;
    private void Awake()
    {
        _settings = SettingsHolder.Instance.GameSettings;
    }
    private void FixedUpdate()
    {
        if (_path != null)
        {
            if (_state == FollowState.Jumping)
            {
                if (!Jumper.IsMoving)
                {
                    _restingTime += Time.fixedDeltaTime;
                    if (_restingTime >= _settings.TimeToHoldStill)
                    {
                        _state = FollowState.Centering;
                        _waddleTarget = LevelCreator.CoordToPos(LevelCreator.PosToCoord(Jumper.transform.position));
                        // _waddleTarget.x += UnityEngine.Random.value * _settings.WaddleVariance;
                    }
                }
                else
                    _restingTime = 0;
            }
            else if (_state == FollowState.Centering)
            {
                // Debug.Log("Waddle waddle");
                Vector2 pos = Jumper.rb.position;
                Jumper.rb.MovePosition(Vector2.MoveTowards(pos, _waddleTarget, _settings.WaddleSpeed));
                if (Vector2.Distance(_waddleTarget, Jumper.rb.position) < 0.01f)
                    _state = FollowState.Ready;
            }
            else if (_state == FollowState.Ready)
            {
                if (targetCoord != LevelCreator.PosToCoord(Jumper.rb.position))
                {
                    // wrong path, has to repath
                    Vector2Int start = LevelCreator.PosToCoord(Jumper.rb.position);
                    Vector2Int goal = LevelCreator.LevelData.GoalPos.Value;
                    _path = GraphSolver.SolveForPath(_graph, start, goal);
                }
                if (_path.Count > 0)
                {
                    GraphEdge edge = _path[0];
                    _path.RemoveAt(0);
                    StartCoroutine(DelayedJump(edge));
                }
                else
                {
                    _path = null;
                }
            }
        }
    }

    private IEnumerator DelayedJump(GraphEdge edge)
    {
        _state = FollowState.Charging;
        Jumper.JumpChargeTrigger();
        yield return new WaitForSeconds(1f);
        Jumper.InstantJump(edge.isDirLeft, edge.jumpStrength);
        targetCoord = edge.dest;
        _state = FollowState.Jumping;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Goal")
        {
            Debug.Log("I have arrived at the goal!");
            _path = null;
        }
    }
    public void PlayPath(MapGraph graph, List<GraphEdge> path)
    {
        if (_startedPlaying)
            return;
        _path = path;
        _graph = graph;
        Jumper = Instantiate(_settings.PlayerPrefab);
        Jumper.transform.position = LevelCreator.GraphCoordToPos(path[0].source);
        _state = FollowState.Ready;
        targetCoord = _path[0].source;
        _startedPlaying = true;
        JumpController.OnGoalReached += ReactToVictory;
    }
    private void ReactToVictory()
    {
        JumpController.OnGoalReached -= ReactToVictory;
        Debug.Log("Victory screeeech!");
        _path = null;
        CameraController.Streamer.TriggerAnim(3);
        StartCoroutine(DelayedVictorySwitch());
    }

    private IEnumerator DelayedVictorySwitch()
    {
        yield return new WaitForSeconds(5f);
        Destroy(Jumper.gameObject);
        GameManager.ChangeGameState(GameState.EditingLevel);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_waddleTarget, 0.7f);
    }
}

enum FollowState
{
    Jumping,
    Centering,
    Charging,
    Ready
}