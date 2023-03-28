using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomMaker : MonoBehaviour
{
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap wallMap;
    [SerializeField] private Room room;

    [SerializeField] private List<Vector2Int> roomCoordinates = new List<Vector2Int>();
    [SerializeField] private List<Vector2Int> roomWalls = new List<Vector2Int>();

    public void MakeRoom()
    {
        roomCoordinates.Clear();
        floorMap.CompressBounds();
        Vector2Int maxPosition = new Vector2Int(Mathf.FloorToInt(floorMap.cellBounds.xMax), Mathf.FloorToInt(floorMap.cellBounds.yMax));
        Vector2Int minPosition = new Vector2Int(Mathf.FloorToInt(floorMap.cellBounds.xMin), Mathf.FloorToInt(floorMap.cellBounds.yMin));
        for (int x = minPosition.x; x <= maxPosition.x; x++)
        {
            for (int y = minPosition.y; y <= maxPosition.y; y++)
            {
                TileBase tileCheck = floorMap.GetTile(new Vector3Int(x, y, 0));
                if (tileCheck != null)
                {
                    roomCoordinates.Add(new Vector2Int(x, y));
                }
            }
        }
        room.minPosition = minPosition;
        room.maxPosition = maxPosition;
        room.midPosition = (maxPosition + minPosition) / 2;
        room.SetRoomCoordinates(roomCoordinates);
        MakeWalls();
    }

    private void MakeWalls()
    {
        roomWalls.Clear();
        wallMap.CompressBounds();
        Vector2Int maxPosition = new Vector2Int(Mathf.FloorToInt(floorMap.cellBounds.xMax), Mathf.FloorToInt(floorMap.cellBounds.yMax));
        Vector2Int minPosition = new Vector2Int(Mathf.FloorToInt(floorMap.cellBounds.xMin), Mathf.FloorToInt(floorMap.cellBounds.yMin));
        for (int x = minPosition.x; x <= maxPosition.x; x++)
        {
            for (int y = minPosition.y; y <= maxPosition.y; y++)
            {
                TileBase tileCheck = wallMap.GetTile(new Vector3Int(x, y, 0));
                if (tileCheck != null)
                {
                    roomWalls.Add(new Vector2Int(x, y));
                }
            }
        }
        room.SetWallCoordinates(roomWalls);
    }
}
