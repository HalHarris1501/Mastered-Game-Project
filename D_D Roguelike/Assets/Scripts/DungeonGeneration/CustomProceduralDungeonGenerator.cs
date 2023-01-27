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
    [SerializeField] [Min(1)] int minimumRooms;


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
        List<Room> roomsInDungeon = new List<Room>();
        floor = CreateStartAndEndRooms(dungeonArea, borderTiles, roomsInDungeon);
        floor.UnionWith(AttemptOtherRooms(dungeonArea, floor, borderTiles, roomsInDungeon));
        HashSet<Vector2Int> corridors = ConnectRooms(dungeonArea, floor, borderTiles, roomsInDungeon);

        floor.UnionWith(corridors);

        
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        tilemapVisualizer.PaintFloorTiles(floor);
    }

    private HashSet<Vector2Int> ConnectRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> floor, HashSet<Vector2Int> offsetTiles, List<Room> roomsList)
    {
        HashSet<Vector2Int> walkableArea = GetWalkableArea(floor, dungeonArea, offsetTiles);
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        corridors = CreateCorridors(walkableArea, roomsList, dungeonArea);

        return corridors;
    }

    private HashSet<Vector2Int> CreateDoors(List<Room> roomsInFloor)
    {
        HashSet<Vector2Int> doors = new HashSet<Vector2Int>();

        foreach (var room in roomsInFloor)
        {
            foreach (var door in room.GetDoorCoordinates())
            {
                doors.Add(door);
            }
        }

        return doors;
    }

    private HashSet<Vector2Int> CreateCorridors(HashSet<Vector2Int> walkableArea, List<Room> roomsList, HashSet<Vector2Int> dungeonArea)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        List<Room> roomsConnected = new List<Room>();
        List<Room> roomsToConnect = roomsList;
        Room endRoom = roomsToConnect[1];
        roomsToConnect.RemoveAt(1);
        roomsToConnect.Add(endRoom);
        int numOfRooms = roomsToConnect.Count;

        for (int i = 0; i < 10; i++)
        {
            HashSet<Vector2Int> tempWalkableArea = walkableArea;
            AddAreaAroundDoors(tempWalkableArea, roomsToConnect);
            List<Vector2Int> doors = new List<Vector2Int>();
            for (int j = 0; j < roomsToConnect.Count; j++)
            {
                foreach (var door in roomsToConnect[i].GetDoorCoordinates())
                {
                    doors.Add(door);
                }
            }
            Vector2Int startDoor = roomsToConnect[0].GetDoorCoordinates()[Random.Range(0, roomsToConnect[0].GetDoorCoordinates().Count)];
            corridors.Add(startDoor);
            doors.Remove(startDoor);
            HashSet<Vector2Int> corridor = CreateCorridor(startDoor, doors, tempWalkableArea, dungeonArea, roomsConnected, roomsToConnect);
            corridors.UnionWith(corridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int startPosition, List<Vector2Int> goalPositions, HashSet<Vector2Int> walkableArea, HashSet<Vector2Int> dungeonArea, List<Room> roomsConnected, List<Room> roomsToConnect)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> tileParents = new Dictionary<Vector2Int, Vector2Int>();
        
        Vector2Int currentPosition = FindShortestPathDFS(startPosition, goalPositions, tileParents, walkableArea);

        if (currentPosition == startPosition)
        {
            Debug.LogError("path impossible");
        }            
        else if (goalPositions.Contains(currentPosition))            
        {                
            goalPositions.Remove(currentPosition);
            for (int i = 0; i < roomsToConnect.Count; i++)
            {
                if(roomsToConnect[i].GetDoorCoordinates().Contains(currentPosition))
                {
                    roomsConnected.Add(roomsToConnect[i]);
                    roomsToConnect.Remove(roomsToConnect[i]);
                }
            }
            while (currentPosition != startPosition)                
            {                    
                corridor.Add(currentPosition);                    
                currentPosition = tileParents[currentPosition];                
            }                

            goalPositions.Remove(startPosition);            
        }
        

        return corridor;
    }

    private Vector2Int FindShortestPathDFS(Vector2Int startPosition, List<Vector2Int> goalPositions, Dictionary<Vector2Int, Vector2Int> tileParents, HashSet<Vector2Int> walkableArea)
    {
        Stack<Vector2Int> tileQueue = new Stack<Vector2Int>();
        HashSet<Vector2Int> exploredTiles = new HashSet<Vector2Int>();
        tileQueue.Push(startPosition);

        while(tileQueue.Count != 0)
        {
            Vector2Int currentTile = tileQueue.Pop();
            Debug.Log("current Tile: " + currentTile);

            if(goalPositions.Contains(currentTile))
            {
                return currentTile;
            }

            IList<Vector2Int> tiles = GetNeighbours(currentTile, walkableArea);

            foreach (var tile in tiles)
            {
                if(!exploredTiles.Contains(tile))
                {
                    //mark tile as explored
                    exploredTiles.Add(tile);

                    //store a reference of the previous tile
                    tileParents.Add(tile, currentTile);

                    //add this to the queue of tiles to examine
                    tileQueue.Push(tile);
                }
            }
        }

        return startPosition;
    }

    private IList<Vector2Int> GetNeighbours(Vector2Int currentTile, HashSet<Vector2Int> walkableArea)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach (var direction in Direction2D.cardinalDirectionsList)
        {
            if(walkableArea.Contains(currentTile + direction))
            {
                neighbours.Add(currentTile + direction);
            }
        }

        return neighbours;
    }

    private void AddAreaAroundDoors(HashSet<Vector2Int> walkableArea, List<Room> roomsList )
    {
        foreach (var room in roomsList)
        {
            foreach (var door in room.GetDoorCoordinates())
            {
                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    if (!room.GetRoomCoordinates().Contains(door + direction))
                    {
                        for (int i = 0; i < roomOffset * 3; i++)
                        {
                            if (!walkableArea.Contains(door + (direction * i)))
                            {
                                walkableArea.Add(door + (direction * i));
                            }
                        }
                    }
                }
            }
        }
    }

    private HashSet<Vector2Int> GetWalkableArea(HashSet<Vector2Int> floor, HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> offsetTiles)
    {
        HashSet<Vector2Int> walkableArea = new HashSet<Vector2Int>();
        walkableArea.UnionWith(dungeonArea);
        foreach (var position in dungeonArea)
        {
            if (position.x == 0 )
            {
                for (int i = 0; i < 10; i++)
                {
                    walkableArea.Add(position + (new Vector2Int(-1, 0) * i));      
                }                 
            }
            else if(position.x == maxSize.x)
            {
                for (int i = 0; i < 10; i++)
                {
                    walkableArea.Add(position + (new Vector2Int(1, 0) * i));
                }
            }
            else if(position.y == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    walkableArea.Add(position + (new Vector2Int(0, -1) * i));
                }
            }
            else if (position.y == maxSize.y)
            {
                for (int i = 0; i < 10; i++)
                {
                    walkableArea.Add(position + (new Vector2Int(0, 1) * i));
                }
            }
        }
        foreach (var position in floor)
        {
            if (walkableArea.Contains(position))
            {
                walkableArea.Remove(position);
            }
        }
        foreach (var position in offsetTiles)
        {
            if (walkableArea.Contains(position))
            {
                walkableArea.Remove(position);
            }
        }

        return walkableArea;
    }

    private Vector2Int FindClosestPointTo(Vector2Int startPoint, List<Room> rooms)
    {
        Vector2Int closestPoint = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var room in rooms)
        {
            foreach (var door in room.GetDoorCoordinates())
            {
                float currentDistance = Vector2.Distance(door, startPoint);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    closestPoint = door;
                }
            }
        }
        return closestPoint;
    }

    private HashSet<Vector2Int> AttemptOtherRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> currentFloor, HashSet<Vector2Int> borderTiles, List<Room> roomsList)
    {
        HashSet<Vector2Int> roomsSet = currentFloor;
            for (int i = 0; i < roomGenerationAttemps; i++)
            {
                bool placeable = true;
                Vector2Int originAttempt;
                int random = Random.Range(0, rooms.Length);
                originAttempt = new Vector2Int(Random.Range(GetMinX(dungeonArea), GetMaxX(dungeonArea)), Random.Range(GetMinY(dungeonArea), GetMaxY(dungeonArea)));
                foreach (var position in rooms[random].GetRoomCoordinates())
                {
                    if (!dungeonArea.Contains(originAttempt + position) || roomsSet.Contains(originAttempt + position) || borderTiles.Contains(originAttempt + position))
                    {
                        placeable = false;
                    }
                }
                if (placeable)
                {
                    roomsSet.UnionWith(AddRoom(rooms[random], originAttempt, borderTiles, roomsList));
                }
            }
        return roomsSet;
    }

    private HashSet<Vector2Int> CreateStartAndEndRooms(HashSet<Vector2Int> dungeonArea, HashSet<Vector2Int> borderTiles, List<Room> roomsList)
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
                startAndEndRooms = AddRoom(startRoom, originAttempt, borderTiles, roomsList);
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
                startAndEndRooms.UnionWith(AddRoom(endRoom, originAttempt, borderTiles, roomsList));
                endPlaced = true;
            }
        }
        return startAndEndRooms;
    }

    private HashSet<Vector2Int> AddRoom(Room room, Vector2Int originPoint, HashSet<Vector2Int> borderTiles, List<Room> roomsList)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        List<Vector2Int> roomPositionsList = new List<Vector2Int>();

        foreach (var position in room.GetRoomCoordinates())
        {
            roomPositions.Add(originPoint + position);
            roomPositionsList.Add(originPoint + position);
        }
        AddToOffset(roomPositions, borderTiles);

        Room newRoom = Instantiate(room);
        newRoom.SetRoomCoordinates(roomPositionsList);
        roomsList.Add(newRoom);

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
                    for (int i = 1; i <= roomOffset; i++)
                    {
                        borderTiles.Add((position + (direction * i)));
                        if(direction == new Vector2Int(0, -1) /*down*/ || direction == new Vector2Int(1, -1) /*right - down*/ || direction == new Vector2Int(-1, -1)/*left - down*/ || direction == new Vector2Int(0, 1) /*up*/ || direction == new Vector2Int(1, 1) /*right - up*/ || direction == new Vector2Int(-1, 1)/*left - up*/)
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
