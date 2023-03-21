using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFiller : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    public void FillDungeon(Vector2Int playerStartPos, List<Room> hostileRooms, int numOfEnemies)
    {
        PlacePlayer(playerStartPos);
        PlaceEnemies(hostileRooms, numOfEnemies);
    }

    private void PlacePlayer(Vector2Int playerStartPos)
    {
        Player.Instance.transform.position = new Vector3(playerStartPos.x + 0.5f, playerStartPos.y + 0.5f, 0);
    }

    private void PlaceEnemies(List<Room> hostileRooms, int numOfEnemies)
    {
        for (int i = 0; i < numOfEnemies; i++)
        {
            int roomToChoose = Random.Range(0, hostileRooms.Count);
            List<Vector2Int> currentRoomCoords = hostileRooms[roomToChoose].GetRoomCoordinates();
            int positionToChoose = Random.Range(0, currentRoomCoords.Count);
            Instantiate(enemies[0], new Vector3(currentRoomCoords[positionToChoose].x + 0.5f, currentRoomCoords[positionToChoose].y + 0.5f, 0), Quaternion.identity);
        }
    }
}
