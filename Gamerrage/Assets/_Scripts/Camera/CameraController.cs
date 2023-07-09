using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] private Transform _background;
    [field: SerializeField] private Transform _middleGound;
    private GameSettings _settings;
    private void Awake()
    {
        _settings = SettingsHolder.Instance.GameSettings;
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

    private void ConstraintToBounds()
    {
        Vector3 position = transform.position;
        if (position.y < _settings.CameraYBounds.x)
        {
            position.y = _settings.CameraYBounds.x;
            transform.position = position;
        }
        if (position.y > _settings.CameraYBounds.y)
        {
            position.y = _settings.CameraYBounds.y;
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

    private void OnDestroy()
    {
    }
}