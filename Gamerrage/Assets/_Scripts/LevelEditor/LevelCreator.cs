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
    public void PlaceBlock(Vector2Int coord, BlockType type)
    {
        BlockPool.Instance.PlaceBlockAt(type, CoordToPos(coord));
    }

    public void CreateData(Vector2Int dimensions)
    {
        LevelData data = new LevelData(dimensions);
        // fill edges with static walls
        for (int x = 0; x < data.sizeX; x++)
        {
            for (int y = 0; y < data.sizeY; y++)
            {
                if (x == 0 || x == data.sizeX - 1 || y == 0 || y == data.sizeY - 1)
                {
                    data[x, y] = BlockType.StaticWalls;
                    PlaceBlock(new(x, y), BlockType.StaticWalls);
                }
            }
        }
        Instance.levelData = data;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}