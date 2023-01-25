using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomProceduralDungeonGenerator : AbstractDuneonGenerator
{
    [SerializeField] private Vector2Int maxSize;
    [SerializeField] private Room startRoom, endRoom;
    [SerializeField] private Room[] rooms;
    [SerializeField] [Min(0)] private int roomOffset;
    [SerializeField] private int roomGenrationAttemps;


    protected override void RunProceduralGeneration()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        HashSet<Vector2Int> dungeonArea = new HashSet<Vector2Int>();
        for (int i = startPosition.x; i < maxSize.x; i++)
        {
            for (int j = startPosition.y; j < maxSize.y; j++)
            {
                dungeonArea.Add(new Vector2Int(i, j));
            }
        }
        CreateRooms(dungeonArea);
    }

    private void CreateRooms(HashSet<Vector2Int> dungeonArea)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> borderTiles = new HashSet<Vector2Int>();
        floor = CreateStartAndEndRooms(dungeonArea);
        floor.UnionWith(AttemptOtherRooms(dungeonArea, floor));
        
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        tilemapVisualizer.PaintFloorTiles(floor);
    }

    private IEnumerable<Vector2Int> AttemptOtherRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> currentFloor)
    {
        HashSet<Vector2Int> roomsSet = currentFloor;
        for (int i = 0; i < roomGenrationAttemps; i++)
        {
            bool placeable = true;
            int random = Random.Range(0, rooms.Length);
            Debug.Log("random: " + random);
            Vector2Int originAttempt = new Vector2Int(Random.Range(GetMinX(dungeonArea), GetMaxX(dungeonArea)), Random.Range(GetMinY(dungeonArea), GetMaxY(dungeonArea)));
            foreach (var position in rooms[random].GetRoomCoordinates())
            {
                if (!dungeonArea.Contains(originAttempt + position) && !roomsSet.Contains(originAttempt + position))
                {
                    placeable = false;
                }
            }
            if(placeable)
            {
                roomsSet.UnionWith(AddRoom(rooms[random], originAttempt));
            }
        }

        return roomsSet;
    }

    private HashSet<Vector2Int> CreateStartAndEndRooms(HashSet<Vector2Int> dungeonArea)
    {
        HashSet<Vector2Int> startAndEndRooms = new HashSet<Vector2Int>();
        bool startPlaced = false;
        while(!startPlaced)
        {
            bool placeable = true;
            Vector2Int originAttempt = new Vector2Int(Random.Range(GetMinX(dungeonArea), GetMaxX(dungeonArea)), Random.Range(GetMinY(dungeonArea), GetMaxY(dungeonArea)));
            foreach (var position in startRoom.GetRoomCoordinates())
            {
                if(!dungeonArea.Contains(originAttempt + position))
                {
                    placeable = false;
                }
            }
            if(placeable)
            {
                startAndEndRooms = AddRoom(startRoom, originAttempt);
                startPlaced = true;
            }
        }
        bool endPlaced = false;
        while (!endPlaced)
        {
            bool placeable = true;
            Vector2Int originAttempt = new Vector2Int(Random.Range(GetMinX(dungeonArea), GetMaxX(dungeonArea)), Random.Range(GetMinY(dungeonArea), GetMaxY(dungeonArea)));
            foreach (var position in endRoom.GetRoomCoordinates())
            {
                if (!dungeonArea.Contains(originAttempt + position) && !startAndEndRooms.Contains(originAttempt + position))
                {
                    placeable = false;
                }
            }
            if (placeable)
            {
                startAndEndRooms.UnionWith(AddRoom(endRoom, originAttempt));
                endPlaced = true;
            }
        }
        return startAndEndRooms;
    }

    private HashSet<Vector2Int> AddRoom(Room room, Vector2Int originPoint)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

        foreach (var position in room.GetRoomCoordinates())
        {
            roomPositions.Add(originPoint + position);
        }

        return roomPositions;
    }

    private int GetMinX(HashSet<Vector2Int> dungeonArea)
    {
        int minX = 0;

        foreach (var position in dungeonArea)
        {
            if(position.x < minX)
            {
                minX = position.x;
            }
        }
        return minX;
    }

    private int GetMinY(HashSet<Vector2Int> dungeonArea)
    {
        int minY = 0;

        foreach (var position in dungeonArea)
        {
            if (position.y < minY)
            {
                minY = position.y;
            }
        }
        return minY;
    }

    private int GetMaxX(HashSet<Vector2Int> dungeonArea)
    {
        int maxX = 0;

        foreach (var position in dungeonArea)
        {
            if (position.x > maxX)
            {
                maxX = position.x;
            }
        }
        return maxX;
    }

    private int GetMaxY(HashSet<Vector2Int> dungeonArea)
    {
        int maxY = 0;

        foreach (var position in dungeonArea)
        {
            if (position.y > maxY)
            {
                maxY = position.y;
            }
        }
        return maxY;
    }
}
