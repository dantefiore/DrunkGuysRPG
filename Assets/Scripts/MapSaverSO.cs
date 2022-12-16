using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapSaverSO : ScriptableObject
{
    public bool fromBattle; // if the player loaded from a battle

    // THINK OF THESE LISTS AS A DICTIONARY
    public List<GameObject> keys;   // the objects in the level
    public List<Vector3> values;    // the location of the objects
}
