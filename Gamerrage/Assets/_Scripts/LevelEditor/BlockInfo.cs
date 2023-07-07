using System;
using System.Collections.Generic;
using UnityEngine;
public static class BlockInfo
{
    public static BlockType GetBlockType(int value) 
    {
        if(!Enum.IsDefined(typeof(BlockType), value))
        {
            Debug.LogError("Tried converting invalid value to enum! Value:" + value);
            throw new InvalidCastException();
        }
        return (BlockType)value;
    }
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