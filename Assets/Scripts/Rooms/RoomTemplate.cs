using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public GameObject[] topRooms;   // rooms with doors at the top
    public GameObject[] bottomRooms;    // rooms with doors at the bottom
    public GameObject[] leftRooms;  // rooms with doors at the left
    public GameObject[] rightRooms; // rooms with doors at the right

    public List<GameObject> rooms;  // rooms that have been spawned

    public float waitTime = 2;  // waits to spawn the boss
    private bool spawnedBoss;   // if the boss is spawned
    public GameObject boss; // temp gameobject to represent the boss

    // the Scriptable Object that saves the map layout
    [SerializeField] MapSaverSO map;    

    private void Start()
    {
        // if the scene loaded from the battle, the map reloades 
        if (map.fromBattle)
        {
            LoadMap();
        }
    }

    private void Update()
    {
        // if it isnt from a battle, then a new map layout is spawned
        if (map.fromBattle)
            return;

        if(waitTime <= 0 && !spawnedBoss)
        {
            // finds what room to spawn the boss
            Vector3 whereToSpawn = new Vector3();
            whereToSpawn = rooms[rooms.Count - 1].transform.position;

            // makes sure the last room isnt a closed room
            if (rooms[rooms.Count - 1].CompareTag("ClosedRoom"))
            {
                whereToSpawn = rooms[rooms.Count - 2].transform.position;
            }

            // adds the boss to the Map Saver
            map.keys.Add(boss);
            map.values.Add(whereToSpawn);

            // spawns the boss in the center of the last room
            Instantiate(boss, whereToSpawn, Quaternion.identity);
            spawnedBoss = true;
        }
        else if(waitTime > 0)
        {
            // decreases time
            waitTime -= Time.deltaTime;
        }
    }

    void LoadMap()
    {
        // spawns each room where there were before the fight
        for (int i = 0; i < map.keys.Count; i++)
        {
            if (map.keys[i])
                Instantiate(map.keys[i], map.values[i], Quaternion.identity);
        }
    }
}
