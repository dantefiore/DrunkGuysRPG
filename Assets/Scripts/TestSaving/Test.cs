using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] MapSaverSO map;

    [SerializeField] GameObject temp1;
    [SerializeField] GameObject temp2;

    [SerializeField] List<GameObject> spawnPoints;

    private void Start()
    {
        if (map.fromBattle)
        {
            LoadFromSO();
        }else if (!map.fromBattle)
        {
            CreateMap();
        }
    }

    void LoadFromSO()
    {
        Debug.Log("Loading map");

        for (int i = 0; i < map.keys.Count; i++)
        {
            Instantiate(map.keys[i], map.values[i], Quaternion.identity);
        }

        map.fromBattle = false;
    }

    void CreateMap()
    {
        int rng = Random.Range(0, spawnPoints.Count);
        int rng2 = Random.Range(0, spawnPoints.Count);

        Instantiate(temp1, spawnPoints[rng].transform);
        Instantiate(temp2, spawnPoints[rng2].transform);

        map.keys.Clear();
        map.values.Clear();

        map.keys.Add(temp1);
        map.keys.Add(temp2);

        map.values.Add(spawnPoints[rng].transform.position);
        map.values.Add(spawnPoints[rng2].transform.position);
    }
}
