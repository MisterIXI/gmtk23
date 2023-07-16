using UnityEngine;

[CreateAssetMenu(fileName = "UtilitySettings", menuName = "Gamerrage/UtilitySettings", order = 0)]
public class UtilitySettings : ScriptableObject
{
    [field: Header("PoolSettings")]
    [field: SerializeField] [Range(3,100)]public int PoolStartSize;
    [field: Header("PathingSettings")]
    [field: SerializeField][Range(1, 100)] public int PathingAgentCount = 50;
    [field: Header("BlockPrefabs")]
    [field: SerializeField] public NormalBlock NormalBlock;
    [field: SerializeField] public SlantedBlock SlantedLeft;
    [field: SerializeField] public SlantedBlock SlantedRight;
    [field: SerializeField] public StaticBlock StaticBlock;
    [field: SerializeField] public GoalBlock GoalBlock;
    [field: SerializeField] public PlayerBlock PlayerBlock;

}