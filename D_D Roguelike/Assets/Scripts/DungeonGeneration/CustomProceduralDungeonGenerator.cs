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
    [SerializeField] private int roomGenerationAttemps;


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
        floor = CreateStartAndEndRooms(dungeonArea, borderTiles);
        floor.UnionWith(AttemptOtherRooms(dungeonArea, floor, borderTiles));
        HashSet<Vector2Int> corridors = ConnectRooms(dungeonArea, floor);

        
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        tilemapVisualizer.PaintFloorTiles(floor);
    }

    private HashSet<Vector2Int> ConnectRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> floor)
    {
        HashSet<Vector2Int> corridors = CreateCorridors(dungeonArea, floor);
        HashSet<Vector2Int> doors = CreateDoors(dungeonArea, floor, corridors);
        corridors.UnionWith(doors);

        return corridors;
    }

    private HashSet<Vector2Int> CreateDoors(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> floor, HashSet<Vector2Int> corridors)
    {
        HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

        return doors;
    }

    private HashSet<Vector2Int> CreateCorridors(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> floor)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        List<Vector2Int> startPoints = FindCorridorStartPoints(dungeonArea, floor);
        //boo


        return corridors;
    }

    private bool AssesAllPositions(Dictionary<Vector2Int, bool> positionsChecked)
    {
        throw new NotImplementedException();
    }

    private List<Vector2Int> FindCorridorStartPoints(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> floor)
    {
        List<Vector2Int> potentialStartPoints = new List<Vector2Int>();
        foreach (var position in dungeonArea)
        {
            if(!floor.Contains(position))
            {
                foreach (var direction in Direction2D.eightDirectionsList)
                {
                    if (!floor.Contains(position + direction))
                    {
                        potentialStartPoints.Add(position + direction);
                    }   
                }
            }
        }
        return potentialStartPoints;
    }

    private HashSet<Vector2Int> AttemptOtherRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> currentFloor, HashSet<Vector2Int> borderTiles)
    {
        HashSet<Vector2Int> roomsSet = currentFloor;
        for (int i = 0; i < roomGenerationAttemps; i++)
        {
            bool placeable = true;
            int random = Random.Range(0, rooms.Length);
            Vector2Int originAttempt = new Vector2Int(Random.Range(GetMinX(dungeonArea), GetMaxX(dungeonArea)), Random.Range(GetMinY(dungeonArea), GetMaxY(dungeonArea)));
            foreach (var position in rooms[random].GetRoomCoordinates())
            {
                if (!dungeonArea.Contains(originAttempt + position) || roomsSet.Contains(originAttempt + position) || borderTiles.Contains(originAttempt + position))
                {
                    placeable = false;
                }
            }
            if(placeable)
            {
                roomsSet.UnionWith(AddRoom(rooms[random], originAttempt, borderTiles));
            }
        }

        return roomsSet;
    }

    private HashSet<Vector2Int> CreateStartAndEndRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> borderTiles)
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
                startAndEndRooms = AddRoom(startRoom, originAttempt, borderTiles);
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
                if (!dungeonArea.Contains(originAttempt + position) || startAndEndRooms.Contains(originAttempt + position) || borderTiles.Contains(originAttempt + position))
                {
                    placeable = false;
                }
            }
            if (placeable)
            {
                startAndEndRooms.UnionWith(AddRoom(endRoom, originAttempt, borderTiles));
                endPlaced = true;
            }
        }
        return startAndEndRooms;
    }

    private HashSet<Vector2Int> AddRoom(Room room, Vector2Int originPoint, HashSet<Vector2Int> borderTiles)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

        foreach (var position in room.GetRoomCoordinates())
        {
            roomPositions.Add(originPoint + position);
        }

        AddToOffset(roomPositions, borderTiles);

        return roomPositions;
    }

    private void AddToOffset(HashSet<Vector2Int> currentRoom, HashSet<Vector2Int> borderTiles)
    {
        foreach (var position in currentRoom)
        {
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                if(!currentRoom.Contains(position + direction))
                {
                    for (int i = 1; i < roomOffset + 1; i++)
                    {
                        borderTiles.Add((position + (direction * i)));
                        if(direction == new Vector2Int(0, -1) || direction == new Vector2Int(0, 1))
                        {
                            borderTiles.Add((position + (direction * (i + 1))));
                        }
                    }
                }
            }
        }
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
