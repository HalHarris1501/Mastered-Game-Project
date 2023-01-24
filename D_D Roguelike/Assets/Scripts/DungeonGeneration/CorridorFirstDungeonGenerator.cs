using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] private int corridorLength = 14, corridorCount = 5;
    [SerializeField] [Range(0.1f, 1f)] private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        HashSet<Vector2Int> floorBottoms = AddBottomToRooms(floorPositions);
        floorPositions.UnionWith(floorBottoms);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
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

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if(roomFloors.Contains(position) == false) //checks that the dead end doesn't already have a room there
            {
                var room = RunRandomWalk(randomWalkParameters, position); //makes a room at the dead end
                roomFloors.UnionWith(room); //prevents duplicates
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if(floorPositions.Contains(position + direction)) //checks if the current position has neighbours in each cardinal direction
                {
                    neighboursCount++;
                }
            }
            if(neighboursCount == 1)
            {
                deadEnds.Add(position); //if it only has one neighbour, then it must be a dead end
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList(); //picks random locations to generate rooms by giving them all ID's then taking the amount specified by roomToCreateCount

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition); //generate rooms at the selected positions
            roomPositions.UnionWith(roomFloor); //avoid duplicates
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1]; //sets position to the last position of the corridor
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor); 
        }
    }
}
