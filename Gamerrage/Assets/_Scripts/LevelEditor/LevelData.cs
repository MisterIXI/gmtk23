using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public LevelData(Vector2Int size)
    {
        blockdata = new int[size.x, size.y];
    }
    public int sizeX => blockdata.GetLength(0);
    public int sizeY => blockdata.GetLength(1);
    public BlockType this[Vector2Int coord]
    {
        get { return (BlockType)blockdata[coord.x, coord.y]; }
        set { blockdata[coord.x, coord.y] = (int)value; }
    }

    public BlockType this[int x, int y]
    {
        get { return (BlockType)blockdata[x, y]; }
        set { blockdata[x, y] = (int)value; }
    }
    public int[,] blockdata;
}
