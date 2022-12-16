using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    //1 --> top spawn point
    //2 --> bottom spawn point
    //3 --> right spawn point
    //4 --> left spawn point
    public int openingDir;

    private RoomTemplate templates; // the rooms
    int rng;    // which room is spawned
    public bool spawned;    // if the room is spawned
    public float waitTime = 4f; // the time before the spawn points are destroyed

    [SerializeField] GameObject closedRoom;

    [SerializeField] MapSaverSO map;

    private void Start()
    {
        // if the player spawned from a battle
        if (!map.fromBattle)
        {
            CreateMap();
        }
        else
            return;

    }

    void CreateMap()
    {
        // destroys this spawnpoint and spawn a new room in 0.1 seconds
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        // if this point spawned a room do nothing
        if (spawned)
            return;

        // the spawned room
        GameObject tempRoom;

        if(openingDir == 1)
        {
            // need to spawn a room with a BOTTOM door
            // randomly picks a room that has an open door on the bottom
            rng = Random.Range(0, templates.bottomRooms.Length);
            tempRoom = Instantiate(templates.bottomRooms[rng], transform.position, templates.bottomRooms[rng].transform.rotation);

            // adds this the saver
            AddToLists(templates.bottomRooms[rng], tempRoom);
        }
        else if(openingDir == 2)
        {
            // need to spawn a room with a TOP door
            // randomly picks a room that has an open door on the bottom
            rng = Random.Range(0, templates.topRooms.Length);
            tempRoom = Instantiate(templates.topRooms[rng], transform.position, templates.topRooms[rng].transform.rotation);

            // adds this the saver
            AddToLists(templates.topRooms[rng], tempRoom);
        }
        else if (openingDir == 3)
        {
            // need to spawn a room with a LEFT door
            // randomly picks a room that has an open door on the bottom
            rng = Random.Range(0, templates.leftRooms.Length);
            tempRoom = Instantiate(templates.leftRooms[rng], transform.position, templates.leftRooms[rng].transform.rotation);

            // adds this the saver
            AddToLists(templates.leftRooms[rng], tempRoom);
        }
        else if (openingDir == 4)
        {
            // need to spawn a room with a RIGHT door
            // randomly picks a room that has an open door on the bottom
            rng = Random.Range(0, templates.rightRooms.Length);
            tempRoom = Instantiate(templates.rightRooms[rng], transform.position, templates.rightRooms[rng].transform.rotation);

            // adds this the saver
            AddToLists(templates.rightRooms[rng], tempRoom);
        }

        spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if there is an opening and a room can't be spawned
        // for whatever reason, a closed room spawns
        if(other.CompareTag("SpawnPoint") && !map.fromBattle)
        {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                // spawn a walls to block off any opening
                Instantiate(closedRoom, transform.position, Quaternion.identity);

                map.keys.Add(closedRoom);
                map.values.Add(transform.position);

                Destroy(gameObject);
            }

            spawned = true;
        }
    }

    void AddToLists(GameObject room, GameObject roomLoc)
    {
        // adds the room and the positions to a list
        map.keys.Add(room);
        map.values.Add(roomLoc.transform.position);
    }
}
