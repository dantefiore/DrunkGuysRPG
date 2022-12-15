using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapSaverSO : ScriptableObject
{
    public bool fromBattle;
    public List<GameObject> keys;
    public List<Vector3> values;
}
