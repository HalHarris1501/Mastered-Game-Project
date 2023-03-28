using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        Vector2Int minPosition = FindMinPosition(floorPositions);
        Vector2Int maxPosition = FindMaxPosition(floorPositions);


        var wallPositions = FindEmptySpace(floorPositions, minPosition, maxPosition);
        CreateBasicWalls(tilemapVisualizer, wallPositions);
    }

    private static Vector2Int FindMinPosition(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int minPosition = new Vector2Int(0, 0);

        foreach (var position in floorPositions)
        {
            if(position.x <= minPosition.x)
            {
                minPosition.x = position.x;
            }
            if(position.y <= minPosition.y)
            {
                minPosition.y = position.y;
            }
        }
        minPosition = new Vector2Int(minPosition.x - 15, minPosition.y - 15);
        return minPosition;
    }

    private static Vector2Int FindMaxPosition(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int maxPosition = new Vector2Int(0, 0);

        foreach (var position in floorPositions)
        {
            if (position.x >= maxPosition.x)
            {
                maxPosition.x = position.x;
            }
            if(position.y >= maxPosition.y)
            {
                maxPosition.y = position.y;
            }
        }
        maxPosition = new Vector2Int(maxPosition.x + 15, maxPosition.y + 15);
        return maxPosition;
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if(floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position);
        }
    }

    public static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions)
    {
        foreach (var position in basicWallPositions)
        {            
            tilemapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction; //checks the direction of the neighbouring tiles
                if(floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);                    
                }
            }
        }
        return wallPositions;
    }

    private static HashSet<Vector2Int> FindEmptySpace(HashSet<Vector2Int> floorPositions, Vector2Int minPosition, Vector2Int maxPosition)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        for (int i = minPosition.x; i < maxPosition.x; i++)
        {
            for (int j = minPosition.y; j < maxPosition.y; j++)
            {
                if(!floorPositions.Contains(new Vector2Int(i, j)))
                {
                    wallPositions.Add(new Vector2Int(i, j));
                    wallPositions.Add(new Vector2Int(i, j + 1));
                }
            }
        }
        return wallPositions;
    }
}
