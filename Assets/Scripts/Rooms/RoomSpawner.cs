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

    private RoomTemplate templates;
    int rng;
    public bool spawned;
    public float waitTime = 4f;

    [SerializeField] GameObject closedRoom;

    [SerializeField] MapSaverSO map;

    private void Start()
    {
        if (!map.fromBattle)
        {
            CreateMap();
        }
        else
            return;

    }

    void CreateMap()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        Invoke("Spawn", 0.1f);
    }

    void LoadFromSO()
    {
        Debug.Log("Loading map");

        for (int i = 0; i < map.keys.Count; i++)
        {
            Instantiate(map.keys[i], map.values[i], Quaternion.identity);
        }

        //map.fromBattle = false;
    }

    private void Spawn()
    {
        if (spawned)
            return;

        //map.keys.Clear();
        //map.values.Clear();

        GameObject tempRoom;

        if(openingDir == 1)
        {
            // need to spawn a room with a BOTTOM door
            rng = Random.Range(0, templates.bottomRooms.Length);
            tempRoom = Instantiate(templates.bottomRooms[rng], transform.position, templates.bottomRooms[rng].transform.rotation);
            AddToLists(templates.bottomRooms[rng], tempRoom);
        }
        else if(openingDir == 2)
        {
            // need to spawn a room with a TOP door
            rng = Random.Range(0, templates.topRooms.Length);
            tempRoom = Instantiate(templates.topRooms[rng], transform.position, templates.topRooms[rng].transform.rotation);
            AddToLists(templates.topRooms[rng], tempRoom);
        }
        else if (openingDir == 3)
        {
            // need to spawn a room with a LEFT door
            rng = Random.Range(0, templates.leftRooms.Length);
            tempRoom = Instantiate(templates.leftRooms[rng], transform.position, templates.leftRooms[rng].transform.rotation);
            AddToLists(templates.leftRooms[rng], tempRoom);
        }
        else if (openingDir == 4)
        {
            // need to spawn a room with a RIGHT door
            rng = Random.Range(0, templates.rightRooms.Length);
            tempRoom = Instantiate(templates.rightRooms[rng], transform.position, templates.rightRooms[rng].transform.rotation);
            AddToLists(templates.rightRooms[rng], tempRoom);
        }

        spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
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
        map.keys.Add(room);
        map.values.Add(roomLoc.transform.position);
    }
}
