using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Gamerrage/GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    [field: Header("Jump settings")]
    [field: SerializeField] public Vector2 JumpStrengthRange { get; private set; }
    [field: SerializeField][field: Range(0.01f, 1f)] public float JumpChargeRatePerSecond { get; private set; }
    [field: SerializeField][field: Range(1, 10)] public int AutoJumpTestSteps { get; private set; }
    [field: SerializeField] public JumpController PlayerPrefab { get; private set; }
    [field: SerializeField] public float TimeToHoldStill { get; private set; } = 1f;
    [field: SerializeField] public float WaddleSpeed { get; private set; } = 4f;
    [field: SerializeField] public float WaddleVariance { get; private set; } = 0.1f;
    [field: Header("Animation Settings")]
    [field: SerializeField][field: Range(0f, 20f)] public float FallingDistressThreshhold { get; private set; } = 0.5f;
    [field: SerializeField][field: Range(0f, 20f)] public float FallingSplatThreshhold { get; private set; } = 1f;
    [field: Header("Camera settings")]
    [field: SerializeField][field: Range(1, 50)] public int PixelScrollZone { get; private set; } = 15;
    [field: SerializeField][field: Range(0.01f, 100f)] public float ScrollSpeed { get; private set; } = 10f;
    [field: SerializeField] public Vector2 CameraYBoundsEditor { get; private set; } = new(6.84f, 47f);
    [field: SerializeField] public Vector2 CameraYBoundsStreamer { get; private set; } = new(4.83f, 46.12f);
    [field: SerializeField][field: Range(0.01f, 1f)] public float BackGroundParallaxMult = 0.7f;
    [field: SerializeField][field: Range(0.01f, 1f)] public float MiddleGroundParallaxMult = 0.3f;

}