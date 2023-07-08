using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    private UtilitySettings utilSettings;
    public static LevelCreator Instance;
    public LevelData levelData { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        utilSettings = SettingsHolder.Instance.UtilitySettings;
    }
    public static Vector3 CoordToPos(Vector2Int coord) => new Vector3(coord.x + 0.5f, coord.y + 0.5f, 0);
    public static Vector2Int PosToCoord(Vector3 pos) => new Vector2Int((int)pos.x, (int)pos.y);

    public void CreateData(Vector2Int dimensions)
    {
        CleanUpBlocks();
        LevelData data = new LevelData(dimensions);
        // fill edges with static walls
        for (int x = 0; x < data.sizeX; x++)
        {
            for (int y = 0; y < data.sizeY; y++)
            {
                if (x == 0 || x == data.sizeX - 1 || y == 0 || y == data.sizeY - 1)
                {
                    data.SetBlock(x, y, BlockType.StaticWalls);
                }
            }
        }
        levelData = data;
    }

    public void CleanUpBlocks()
    {
        if (levelData != null)
        {
            for (int x = 0; x < levelData.sizeX; x++)
            {
                for (int y = 0; y < levelData.sizeY; y++)
                {
                    BlockPool.Instance.ReturnBlock(levelData, new(x, y));
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
        CleanUpBlocks();
    }
}