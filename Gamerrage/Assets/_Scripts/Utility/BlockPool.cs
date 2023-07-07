using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public static BlockPool Instance;
    private UtilitySettings utilitySettings;
    private Dictionary<BlockType, List<BaseBlock>> pool;
    private PlayerBlock playerBlock;
    private GoalBlock goalBlock;
    private Dictionary<BlockType, BaseBlock> prefabs;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        utilitySettings = SettingsHolder.Instance.UtilitySettings;
        BuildPrefabDict();
        InitPool();
    }

    private void BuildPrefabDict()
    {
        prefabs = new Dictionary<BlockType, BaseBlock>();
        prefabs[BlockType.Player] = utilitySettings.PlayerBlock;
        prefabs[BlockType.Goal] = utilitySettings.GoalBlock;
        prefabs[BlockType.Normal] = utilitySettings.NormalBlock;
        prefabs[BlockType.SlantLeft] = utilitySettings.SlantedLeft;
        prefabs[BlockType.SlantRight] = utilitySettings.SlantedRight;
        prefabs[BlockType.StaticWalls] = utilitySettings.StaticBlock;
    }
    private void InitPool()
    {
        pool = new Dictionary<BlockType, List<BaseBlock>>();
        foreach (BlockType type in Enum.GetValues(typeof(BlockType)))
        {
            pool[type] = new List<BaseBlock>();
        }
        pool[BlockType.Player].Add(Instantiate(utilitySettings.PlayerBlock));
        pool[BlockType.Player].First().gameObject.SetActive(false);
        pool[BlockType.Goal].Add(Instantiate(utilitySettings.GoalBlock));
        pool[BlockType.Goal].First().gameObject.SetActive(false);
        for (int i = 0; i < utilitySettings.PoolStartSize; i++)
        {
            foreach (BlockType type in prefabs.Keys)
            {
                // add block for each placeable Blocktype
                if ((int)type > 0)
                    addBlock(type);
            }
        }
    }

    private void addBlock(BlockType type)
    {
        var newObject = Instantiate(prefabs[type]);
        newObject.gameObject.SetActive(false);
        pool[type].Add(newObject);
        newObject.transform.parent = transform;
    }

    public BaseBlock PlaceBlockAt(BlockType type, Vector3 pos)
    {
        BaseBlock block;
        if (type == BlockType.Goal)
        {
            if (goalBlock == null)
            {
                goalBlock = (GoalBlock)pool[type].First();
                pool[type].RemoveAt(0);
                block = goalBlock;
            }
            else
            {
                block = playerBlock;
            }
        }
        else if (type == BlockType.Player)
        {
            if (playerBlock == null)
            {
                playerBlock = (PlayerBlock)pool[type].First();
                pool[type].RemoveAt(0);
                block = playerBlock;
            }
            else
            {
                block = playerBlock;
            }
        }
        else
        {
            if (pool[type].Count < 1)
                addBlock(type);
            block = pool[type].First();
            pool[type].RemoveAt(0);
        }
        block.transform.position = pos;
        block.gameObject.SetActive(true);
        block.transform.parent = null;
        return block;
    }

    public void ReturnBlock(BlockType type, BaseBlock block)
    {
        if (type == 0 || block == null)
            return;
        block.gameObject.SetActive(false);
        block.transform.parent = transform;
        pool[type].Add(block);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}