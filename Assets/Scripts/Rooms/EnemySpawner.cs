using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;    // the prefab of the enemy

    // the list of posible enemies in the battle
    [SerializeField] List<CharacterStatus> enemies = new List<CharacterStatus>();
    List<CharacterStatus> posibleEnemies;

    [SerializeField] Party party;   // the enemy party
    [SerializeField] MapSaverSO saver;

    [Header("Spawn Points")]    // where the enemy can spawn in a room
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        // is the player didn't spawn from a battle
        if (!saver.fromBattle)
        {
            RefillList();
            Invoke("SpawnParty", 0.5f);
        }
    }

    void SpawnParty()
    {
        // how many enemies can spawn in the room
        int rand_enemy_amt = Random.Range(1, spawnPoints.Count+1);

        //Debug.Log(rand_enemy_amt);

        for (int j = 0; j < rand_enemy_amt; j++)
        {
            // where the enemy will spawn
            int rand_spawn = Random.Range(0, spawnPoints.Count);

            // how big the enemy party will be
            int rand_party_size = Random.Range(1, 5);

            // spawn the prefab and gets a reference to it and its StatusManager Script
            Instantiate(enemyPrefab, spawnPoints[rand_spawn]);
            StatusManager enemyStatusManager = enemyPrefab.GetComponent<StatusManager>();

            //clears the party in that script
            enemyStatusManager.enemies.Clear();

            // adds random enemies to the list in the status manager
            for (int i = 0; i < rand_party_size; i++)
            {
                if(posibleEnemies.Count > 0)
                {
                    int rng = Random.Range(0, posibleEnemies.Count);

                    enemyStatusManager.enemies.Add(posibleEnemies[rng]);
                    posibleEnemies.Remove(posibleEnemies[rng]);
                }
            }

            // adds these enemies to the saver lists
            saver.keys.Add(enemyPrefab);
            saver.values.Add(spawnPoints[rand_spawn].position);

            RefillList();
        }
    }

    void RefillList()
    {
        posibleEnemies = new List<CharacterStatus>(enemies);
    }
}
