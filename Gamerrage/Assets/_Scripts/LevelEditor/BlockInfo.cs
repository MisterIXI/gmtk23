using System;
using System.Collections.Generic;
using UnityEngine;
public static class BlockInfo
{
    public static BlockType GetBlockType(int value)
    {
        if (!Enum.IsDefined(typeof(BlockType), value))
        {
            Debug.LogError("Tried converting invalid value to enum! Value:" + value);
            throw new InvalidCastException();
        }
        return (BlockType)value;
    }

    public static bool IsGroundBlock(BlockType type)
    {
        if (type == BlockType.Normal || type == BlockType.StaticWalls)
            return true;
        return false;
    }
    public static bool IsRemovableBlock(BlockType type)
    {
        if (type == BlockType.Empty)
            return false;
        if (type == BlockType.StaticWalls)
            return false;
        return true;
    }
    public static bool IsGoalOrPlayer(BlockType type) => type == BlockType.Goal || type == BlockType.Player;
}

public enum BlockType
{
    Goal = -2,
    Player = -1,
    Empty = 0,
    Normal = 1,
    SlantLeft = 2,
    SlantRight = 3,
    StaticWalls = 100,


}