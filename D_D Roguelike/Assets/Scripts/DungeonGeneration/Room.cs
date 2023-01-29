using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Vector2Int minPosition, maxPosition;
    [SerializeField] private List<Vector2Int> roomCoordinates = new List<Vector2Int>();
    [SerializeField] private List<Vector2Int> doorCoordinates = new List<Vector2Int>();
    
    public void SetRoomCoordinates(List<Vector2Int> positions)
    {
        roomCoordinates.Clear();
        roomCoordinates = positions;
    }

    public List<Vector2Int> GetRoomCoordinates()
    {
        if (roomCoordinates.Count == 0)
        {

            roomCoordinates.Clear();
            int counter = 0;
            for (int x = minPosition.x; x < maxPosition.x; x++)
            {
                for (int y = minPosition.y; y < maxPosition.y; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    roomCoordinates.Add(position);
                    counter++;
                }
            }
        }
        return roomCoordinates;     
    }

    public void SetDoorCoordinates(List<Vector2Int> doorPositions)
    {
        doorCoordinates.Clear();
        doorCoordinates = doorPositions;
    }

    public List<Vector2Int> GetDoorCoordinates()
    {
        if(!(doorCoordinates.Count > 0))
        {
            foreach (var position in roomCoordinates)
            {
                int neighbours = 0;
                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    if(roomCoordinates.Contains(position + direction))
                    {
                        neighbours++;
                    }
                }
                if(neighbours == 3)
                {
                    doorCoordinates.Add(position);
                }
            }
        }
        return doorCoordinates;
    }


}
