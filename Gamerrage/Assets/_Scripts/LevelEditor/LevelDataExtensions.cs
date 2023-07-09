using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


static class LevelDataExtensions
{
    public static bool IsAboveFree(this LevelData levelData, Vector2Int coord) => levelData[coord + Vector2Int.up] <= 0;
    public static bool IsGrounded(this LevelData levelData, Vector2Int coord) =>
        BlockInfo.IsGroundBlock(levelData[coord + Vector2Int.down]);
    public static bool IsValidBlockPos(this LevelData levelData, Vector2Int coord, BlockType type)
    {
        if (type == BlockType.Empty)
            return false;
        if (levelData[coord] == BlockType.StaticWalls)
            return false;
        // grounded check for special blocks
        if (BlockInfo.IsGoalOrPlayer(type) && !levelData.IsGrounded(coord))
            return false;
        // check if this would unground goal or player
        if (!BlockInfo.IsGroundBlock(type) && BlockInfo.IsGoalOrPlayer(levelData[coord + Vector2Int.up]))
            return false;
        return true;
    }

    public static bool HasGoalAndPlayerBlocks(this LevelData levelData)
    {
        bool foundPlayer = false;
        bool foundGoal = false;
        for (int x = 1; x < levelData.sizeX - 1; x++)
        {
            for (int y = 1; y < levelData.sizeY - 1; y++)
            {
                if (levelData[x, y] == BlockType.Goal)
                    foundGoal = true;
                else if (levelData[x, y] == BlockType.Player)
                    foundPlayer = true;
                if (foundGoal && foundPlayer)
                    return true;
            }
        }
        return false;
    }
}