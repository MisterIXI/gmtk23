using UnityEngine;


public class LevelCreator : MonoBehaviour
{
    public static bool IsReady => Instance != null && Instance._levelData != null;
    private UtilitySettings utilSettings;
    public static LevelCreator Instance;
    public static LevelData LevelData => Instance._levelData;
    private LevelData _levelData;
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
    private void Start() {
        CreateData(new(10,10));
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
        _levelData = data;
    }

    public void CleanUpBlocks()
    {
        if (_levelData != null && BlockPool.Instance != null)
        {
            for (int x = 0; x < _levelData.sizeX; x++)
            {
                for (int y = 0; y < _levelData.sizeY; y++)
                {
                    BlockPool.Instance.ReturnBlock(_levelData, new(x, y));
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