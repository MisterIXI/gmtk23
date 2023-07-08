using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public Vector2Int? PlayerPos;
    public Vector2Int? GoalPos;
    public LevelData(Vector2Int size)
    {
        blockdata = new VectorArray<int>(size);
        blocks = new VectorArray<BaseBlock>(size);
    }
    public int sizeX => blockdata.GetLength(0);
    public int sizeY => blockdata.GetLength(1);
    public BlockType this[Vector2Int coord]
    {
        get { return (BlockType)blockdata[coord.x, coord.y]; }
        //set { blockdata[coord.x, coord.y] = (int)value; }
    }

    public BlockType this[int x, int y]
    {
        get { return (BlockType)blockdata[x, y]; }
        //set { blockdata[x, y] = (int)value; }
    }

    public void SetBlock(Vector2Int coord, BlockType type) => SetBlock(coord.x, coord.y, type);
    public void SetBlock(int x, int y, BlockType type)
    {
        // check if goal or player are overwritten
        if(this[x,y] == BlockType.Goal)
            GoalPos = null;
        if(this[x,y] == BlockType.Player)
            PlayerPos = null;
        
        // check if goal or player are being set
        if(type == BlockType.Goal && GoalPos != null)
            SetBlock(GoalPos.Value, BlockType.Empty);
        if(type==BlockType.Player && PlayerPos != null)
            SetBlock(PlayerPos.Value, BlockType.Empty);

        // give back previous block
        BlockPool.Instance.ReturnBlock(this[x,y], blocks[x,y]);

        // update block arrays
        BaseBlock block = BlockPool.Instance.PlaceBlockAt(type, LevelCreator.CoordToPos(new(x, y)));
        blockdata[x, y] = (int)type;
        blocks[x, y] = block;
    }
    private VectorArray<int> blockdata;
    public VectorArray<BaseBlock> blocks { get; private set; }
}
