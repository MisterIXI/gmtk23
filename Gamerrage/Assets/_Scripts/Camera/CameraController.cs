using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameState;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] private Transform _background;
    [field: SerializeField] private Transform _middleGound;
    private GameSettings _settings;
    private Camera _cam;
    public static CameraController Instance { get; private set; }
    [field: SerializeField] private StreamerAnimator _streamer;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SubscribeEvents();
        _settings = SettingsHolder.Instance.GameSettings;
        _cam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (GameManager.GameState == GameState.EditingLevel)
        {
            Vector2 pos = Input.mousePosition;
            Vector2 dir = Vector2.zero;
            // if (pos.x < _settings.PixelScrollZone)
            //     dir.x = -1;
            // else if (pos.x > Screen.currentResolution.width - _settings.PixelScrollZone)
            //     dir.x = 1;
            if (pos.y < _settings.PixelScrollZone)
                dir.y = -1;
            else if (pos.y > Screen.currentResolution.height - _settings.PixelScrollZone)
                dir.y = 1;
            transform.position += (Vector3)(dir * Time.deltaTime * _settings.ScrollSpeed);
        }
        else if (GameManager.GameState == GameState.StreamerPlaying)
        {
            Vector2 followerPos = MapValidator.FollowerPos;
            Vector3 position = transform.position;
            position.y = followerPos.y;
            transform.position = position;
        }
        ConstraintToBounds();
        ParallaxBackGrounds();
    }
    private void OnGameStateChange(GameState oldState, GameState newState)
    {
        if (newState == StreamerPlaying)
        {
            _cam.orthographicSize = 8;
            Vector3 pos = transform.position;
            pos.x = 14.6f;
            transform.position = pos;
            _streamer.gameObject.SetActive(true);
        }
        else if (oldState == StreamerPlaying)
        {
            _streamer.gameObject.SetActive(false);
            _cam.orthographicSize = 7;
            Vector3 pos = transform.position;
            pos.x = 12.5f;
            transform.position = pos;
        }
    }
    private void ConstraintToBounds()
    {
        Vector2 bounds = GameManager.GameState == StreamerPlaying ? _settings.CameraYBoundsStreamer : _settings.CameraYBoundsEditor;
        Vector3 position = transform.position;
        if (position.y < bounds.x)
        {
            position.y = bounds.x;
            transform.position = position;
        }
        if (position.y > bounds.y)
        {
            position.y = bounds.y;
            transform.position = position;
        }
    }
    private void ParallaxBackGrounds()
    {
        Vector3 pos1 = _background.position;
        Vector3 pos2 = _middleGound.position;
        float y = transform.position.y;
        const int offset = 5;
        pos1.y = (y - offset) * _settings.BackGroundParallaxMult;
        pos2.y = (y - offset) * _settings.MiddleGroundParallaxMult;
        _background.position = pos1;
        _middleGound.position = pos2;
    }

    private void SubscribeEvents()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    private void UnSubscribeEvents()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        UnSubscribeEvents();
    }
}