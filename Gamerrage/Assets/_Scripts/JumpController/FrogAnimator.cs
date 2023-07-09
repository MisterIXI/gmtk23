using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FrogAnimator : MonoBehaviour
{
    [field: SerializeField] private JumpController _jumper;
    [field: SerializeField] private LayerMask _mask;
    private Animator _animator;
    private AnimState _state;
    private bool actuallyFalling;
    private float _maxFallSpeed;
    private float _timeSinceLanding;
    private GameSettings _settings;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _settings = SettingsHolder.Instance.GameSettings;
        SubscribeToEvents();
    }

    private void Update()
    {
        if (_state == AnimState.Jumping)
        {
            if (_jumper.rb.velocity.y < 0.01f)
                _state = AnimState.Falling;
        }
        else if (_state == AnimState.Falling)
        {
            float fallspeed = _jumper.rb.velocity.y;
            if (fallspeed < _maxFallSpeed)
                _maxFallSpeed = fallspeed;
            if (fallspeed < _settings.FallingDistressThreshhold)
                _animator.SetTrigger("Distress");
            var hit = Physics2D.BoxCast(_jumper.rb.position + Vector2.down * 0.5f, new(0.6f, 0.3f), 0, Vector2.down, layerMask: _mask, distance: 0.1f);
            if (hit.collider != null)
            {
                _state = AnimState.Landing;
                if (_maxFallSpeed < _settings.FallingSplatThreshhold)
                    _animator.SetTrigger("Splat");
                else
                    _animator.SetTrigger("Land");
                _timeSinceLanding = Time.time;
            }
        }
        else if (_state == AnimState.Landing)
        {
            if (Time.time - _timeSinceLanding > 1f)
                _state = AnimState.Idle;
        }
    }

    private void OnJumpChargeTrigger()
    {
        _animator.SetTrigger("Charge");
        _state = AnimState.JumpCharge;
    }

    private void OnJumpTrigger()
    {
        _animator.SetTrigger("Jump");
        _state = AnimState.Jumping;
    }
    private void SubscribeToEvents()
    {
        JumpController.OnJump += OnJumpTrigger;
        JumpController.OnStartedCharging += OnJumpChargeTrigger;
    }

    private void UnSubscribeFromEvents()
    {
        JumpController.OnJump -= OnJumpTrigger;
        JumpController.OnStartedCharging -= OnJumpChargeTrigger;
    }
    private void OnDestroy()
    {
        UnSubscribeFromEvents();
    }
    private enum AnimState
    {
        Idle,
        JumpCharge,
        Jumping,
        Falling,
        Landing
    }
}