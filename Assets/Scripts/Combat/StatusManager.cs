using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is on the enemy characters in the overworld */
public class StatusManager : MonoBehaviour
{
    private CharacterStatus playerStatus;

    // the party that the player will fight
    public List<CharacterStatus> enemies = new List<CharacterStatus>();

    // the scene this will open, later might have special scene for bosses
    [SerializeField] string SceneToLoad = "BattleScreen";    

    public Party enemyParty;
    bool isAttacked = false;

    /* WHEN THE PLAYER MAKES CONTACT WITH THIS UNIT, 
     * THE ENEMY PARTY IS CHANGED, AND THE BATTLE SCENE IS LOADED */
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemyParty.partyMembers.Clear();

            //makes the enemy party so the right enemies spawn in the battle
            for (int i = 0; i < enemies.Count; i++)
            {
                enemyParty.partyMembers.Add(enemies[i]);
            }

            //gets the current party leader
            playerStatus = other.GetComponent<StatusHolder>().charStatus;

            if (playerStatus.currHealth > 0)
            {
                if (!isAttacked)
                {
                    isAttacked = true;
                    setBattleData(other);   //sets the player transform;
                    LevelLoader.instance.LoadLevel(SceneToLoad); //loads the battle
                }
            }
        }
    }

    /* SAVES THE PLAYER'S LOCATION */
    private void setBattleData(Collider other)
    {
        // Player Data 
        playerStatus.position[0] = this.transform.position.x;
        playerStatus.position[1] = this.transform.position.y;
    }

    /*
     *  --------------------------------------------------------------
     *          MAY HAVE TO ADD THIS BEFORE LOADING THE BATTLE
     *     (if the scriptable objects aren't changing info properly)
     *  --------------------------------------------------------------
     *  
        public void SaveScriptables()
        {
            //creates a file and saves all scriptable objects
            for(int i =0; i < objects.Count; i++)
            {
                FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.dat", i));
                BinaryFormatter newBinary = new BinaryFormatter();
                var json = JsonUtility.ToJson(objects[i]);
                newBinary.Serialize(file, json);
                file.Close();
            }
        }
     * 
     * -------------------------------------------------------------
     *       MAY HAVE TO ADD THIS AFTER BATTLE SCENE IS LOADED
     * -------------------------------------------------------------
     * 
        public void LoadScriptables()
        {
            //loads all scriptable objects
            for (int i = 0; i < objects.Count; i++)
            {
                if(File.Exists(Application.persistentDataPath + string.Format("/{0}.dat", i)))
                {
                    FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.dat", i), FileMode.Open);

                    BinaryFormatter newBinary = new BinaryFormatter();
                    JsonUtility.FromJsonOverwrite((string)newBinary.Deserialize(file), objects[i]);
                    file.Close();
                }
            }
        }
     */
}
