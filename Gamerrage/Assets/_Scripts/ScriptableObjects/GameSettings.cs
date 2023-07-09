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

}