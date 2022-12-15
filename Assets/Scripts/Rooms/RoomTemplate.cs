using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public List<GameObject> rooms;

    public float waitTime = 2;
    private bool spawnedBoss;
    public GameObject boss;

    [SerializeField] MapSaverSO map;

    private void Start()
    {
        if (map.fromBattle)
        {
            LoadMap();
        }
    }

    private void Update()
    {
        if (map.fromBattle)
            return;

        if(waitTime <= 0 && !spawnedBoss)
        {
            Vector3 whereToSpawn = new Vector3();
            whereToSpawn = rooms[rooms.Count - 1].transform.position;

            if (rooms[rooms.Count - 1].CompareTag("ClosedRoom"))
            {
                whereToSpawn = rooms[rooms.Count - 2].transform.position;
            }

            map.keys.Add(boss);
            map.values.Add(whereToSpawn);

            Instantiate(boss, whereToSpawn, Quaternion.identity);
            spawnedBoss = true;
        }
        else if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    void LoadMap()
    {
        for (int i = 0; i < map.keys.Count; i++)
        {
            Instantiate(map.keys[i], map.values[i], Quaternion.identity);
        }
    }
}
