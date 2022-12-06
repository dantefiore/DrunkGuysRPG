using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField] public CharacterStatus stats;  // the character's stats
    [SerializeField] public List<int> toNextLevel;  // list of how much exp is needed to level up
    private int levelIndex; // the index of what level the player is at

    /* CHECKS IF THE CHARACTER LEVELS UP */
    public void CheckExp()
    {
        Debug.Log("Check exp");

        levelIndex = stats.level - 1;

        /* if the character has enough EXP to level up, then the level increase, 
         * the required exp is subtracted, then StatChanges() is called */
        if (stats.exp >= toNextLevel[levelIndex])
        {
            stats.exp -= toNextLevel[levelIndex];
            stats.level++;
            StatChanges();
        }
    }

    /* Stats are increased */
    private void StatChanges()
    {
        /*------------------------------------------------------------------------------
         * RIGHT NOW THE STATS ARE INCREASE BY LEVEL AMOUNT, MIGHT HAVE TO CHANGE LATER 
         *------------------------------------------------------------------------------*/

        stats.maxHealth += stats.level;
        stats.currHealth += stats.level;
        stats.maxAbilityPts += stats.level;
        stats.currAbilityPts += stats.level;
        stats.strength += stats.level;
        stats.speed += stats.level;
    }
}
