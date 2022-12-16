using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    /* ------------------------------------------
     * This is just a list of all spawned rooms
     * ------------------------------------------ */
    private RoomTemplate templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        templates.rooms.Add(this.gameObject);
    }
}
