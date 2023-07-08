using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


static class LevelDataExtensions
{
    public static bool IsAboveFree(this LevelData levelData, Vector2Int coord) => levelData[coord + Vector2Int.up] <= 0;
    public static bool IsGrounded(this LevelData levelData, Vector2Int coord) =>
        levelData[coord + Vector2Int.down] == BlockType.Normal || levelData[coord + Vector2Int.down] == BlockType.StaticWalls;
}