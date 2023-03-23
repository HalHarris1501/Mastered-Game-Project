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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeRoom()
    {
        roomCoordinates.Clear();
        floorMap.CompressBounds();
        Vector2Int maxPosition = new Vector2Int(floorMap.size.x - 1, floorMap.size.y - 1);
        for (int x = 0; x <= maxPosition.x; x++)
        {
            for (int y = 0; y <= maxPosition.y; y++)
            {
                TileBase tileCheck = floorMap.GetTile(new Vector3Int(x, y, 0));
                if (tileCheck != null)
                {
                    roomCoordinates.Add(new Vector2Int(x, y));
                }
            }
        }
        room.maxPosition = maxPosition;
        room.SetRoomCoordinates(roomCoordinates);
        MakeWalls();
    }

    private void MakeWalls()
    {
        roomWalls.Clear();
        wallMap.CompressBounds();
        Vector2Int maxPosition = new Vector2Int(wallMap.size.x - 1, wallMap.size.y - 1);
        for (int x = 0; x <= maxPosition.x; x++)
        {
            for (int y = 0; y <= maxPosition.y; y++)
            {
                TileBase tileCheck = wallMap.GetTile(new Vector3Int(x, y, 0));
                if (tileCheck != null)
                {
                    roomWalls.Add(new Vector2Int(x, y));
                }
            }
        }
        room.maxPosition = maxPosition;
        room.SetWallCoordinates(roomWalls);
    }
}
