using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int minPosition, maxPosition;
    public List<Vector2Int> roomCoordinates = new List<Vector2Int>();

    public List<Vector2Int> GetRoomCoordinates()
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
        return roomCoordinates;
    }
}
