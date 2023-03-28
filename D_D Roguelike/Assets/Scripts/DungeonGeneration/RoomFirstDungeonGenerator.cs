using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int minRoomWidth, minRoomHeight;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] private int offset = 1;
    [SerializeField] [Range(1, 5)]private int corridorWidth = 1;
    [SerializeField] private RoomType roomType = RoomType.Simple;

    [SerializeField] private Room startRoom;
    [SerializeField] private List<Room> midRooms;
    [SerializeField] private Room endRoom;
    [SerializeField] private Room corridorRoom;


    private enum RoomType
    {
        Random,
        Simple,
        Custom
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.CeilToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        if (roomType == RoomType.Random)
        {
            floor.UnionWith(CreateRoomsRandomly(roomsList));
        }
        else if (roomType == RoomType.Simple)
        {
            floor.UnionWith(CreateSimpleRooms(roomsList));
        }
        else
        {
            floor.UnionWith(CreateCustomRooms(roomsList, walls, floor));
        }      

        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        WallGenerator.CreateBasicWalls(tilemapVisualizer, walls);
        tilemapVisualizer.PaintFloorTiles(floor);
    }

    private HashSet<Vector2Int> CreateCustomRooms(List<BoundsInt> roomsList, HashSet<Vector2Int> walls, HashSet<Vector2Int> currentFloor)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor.UnionWith(CreateStartAndEndRoom(roomsList, walls, currentFloor));

        foreach(var room in roomsList)
        {
            Vector2Int roomCenter = new Vector2Int(Mathf.CeilToInt(room.center.x), Mathf.CeilToInt(room.center.y));
            Room currentRoom = midRooms[Random.Range(0, midRooms.Count)];
            foreach (var position in currentRoom.GetWallCoordinates())
            {
                walls.Add(roomCenter + (position - currentRoom.midPosition));
                if(currentFloor.Contains(roomCenter + (position - currentRoom.midPosition)))
                {
                    currentFloor.Remove(roomCenter + (position - currentRoom.midPosition));
                }
            }
            foreach (var position in currentRoom.GetRoomCoordinates())
            {
                floor.Add(roomCenter + (position - currentRoom.midPosition));
            }
            
        }
        return floor;
    }

    private HashSet<Vector2Int> CreateStartAndEndRoom(List<BoundsInt> roomsList, HashSet<Vector2Int> walls, HashSet<Vector2Int> currentFloor)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        Vector2Int roomCenter = new Vector2Int(Mathf.CeilToInt(roomsList[0].center.x), Mathf.CeilToInt(roomsList[0].center.y));
        Room currentRoom = startRoom;
        foreach (var position in currentRoom.GetWallCoordinates())
        {
            walls.Add(roomCenter + (position - currentRoom.midPosition));
            if (currentFloor.Contains(roomCenter + (position - currentRoom.midPosition)))
            {
                currentFloor.Remove(roomCenter + (position - currentRoom.midPosition));
            }
        }
        foreach (var position in currentRoom.GetRoomCoordinates())
        {
            floor.Add(roomCenter + (position - currentRoom.midPosition));
        }
        roomsList.RemoveAt(0);

        roomCenter = new Vector2Int(Mathf.CeilToInt(roomsList[roomsList.Count - 1].center.x), Mathf.CeilToInt(roomsList[roomsList.Count - 1].center.y));
        currentRoom = endRoom;
        foreach (var position in currentRoom.GetWallCoordinates())
        {
            walls.Add(roomCenter + (position - currentRoom.midPosition));
            if (currentFloor.Contains(roomCenter + (position - currentRoom.midPosition)))
            {
                currentFloor.Remove(roomCenter + (position - currentRoom.midPosition));
            }
        }
        foreach (var position in currentRoom.GetRoomCoordinates())
        {
            floor.Add(roomCenter + (position - currentRoom.midPosition));
        }
        roomsList.RemoveAt(roomsList.Count - 1);

        return floor;
    }

    private HashSet<Vector2Int> AddBottomToRooms(HashSet<Vector2Int> floor)
    {
        HashSet<Vector2Int> floorBottoms = new HashSet<Vector2Int>();
        foreach (var position in floor)
        {
            floorBottoms.Add(position + Vector2Int.down);
        }
        return floorBottoms;
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCentre = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCentre);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        int randomNum = Random.Range(0, roomCenters.Count);
        var currentRoomCenter = roomCenters[randomNum];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closestPoint = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closestPoint);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closestPoint);
            currentRoomCenter = closestPoint;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }          

            foreach (var floorPosition in corridorRoom.GetRoomCoordinates())
            {
                corridor.Add(position + (floorPosition));
            }
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x )
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            
            foreach (var floorPosition in corridorRoom.GetRoomCoordinates())
            {
                corridor.Add(position + (floorPosition));
            }
        }
        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closestPoint = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closestPoint = position;
            }
        }
        return closestPoint;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            for (int column = offset; column < room.size.x - offset; column++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(column, row);
                    floor.Add(position);
                }

            }
        }
        return floor;
    }
}
