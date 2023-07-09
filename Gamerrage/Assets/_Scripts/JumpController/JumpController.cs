using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class JumpController : MonoBehaviour
{
    public static event Action OnStartedCharging;
    public static event Action<float> OnChargeProgressChanged;
    public static event Action OnJump;

    public bool IsMoving => rb.velocity.magnitude > 0.1f;
    public Rigidbody2D rb { get; private set; }
    private bool _isCharging;
    private float _chargeStart;
    private float _currentCharge;
    private GameSettings _settings;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _settings = SettingsHolder.Instance.GameSettings;
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (_isCharging)
        {
            Vector2 range = _settings.JumpStrengthRange;
            _currentCharge = Mathf.Min(range.y, _currentCharge + _settings.JumpChargeRatePerSecond * Time.fixedDeltaTime * Time.timeScale);
            float progress = (_currentCharge - range.x) / (range.y - range.x);
            OnChargeProgressChanged?.Invoke(progress);
        }
    }

    public void StartJumpCharging()
    {
        _chargeStart = Time.time;
        _currentCharge = _settings.JumpStrengthRange.x;
        OnStartedCharging?.Invoke();
    }

    public void ReleaseJump(bool jumpLeft)
    {
        float dir = jumpLeft ? -1 : 1;
        Vector2 force = new(dir * _currentCharge * 0.3f, _currentCharge * 0.6f);
        _isCharging = false;
        _currentCharge = 0;
        rb.AddForce(force, ForceMode2D.Impulse);
        OnJump?.Invoke();
    }

    public void InstantJump(bool jumpLeft, float jumpCharge)
    {
        _currentCharge = jumpCharge;
        ReleaseJump(jumpLeft);
    }

}