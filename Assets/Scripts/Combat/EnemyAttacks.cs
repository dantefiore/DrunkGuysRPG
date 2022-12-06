using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*  This will hold every move the enemy could take, and takes in everything it needs to do its moves.
*   Later down the line for bosses and other enemies will have moves to heal the whole party or attack 
*   the whole enemy party.
*   
*   THIS WILL HAVE TO CHANGE BUT WORKS FOR WHAT IT NEEDS TO DO FOR NOW
*/
public class EnemyAttacks : MonoBehaviour
{
    //Randomly chooses to attack or heal
    public void TakeTurn(List<CharacterStatus> enemies, List<CharacterStatus> players, int charID,
                              List<StatusHUD> playerHUD, List<StatusHUD> enemyHUD, TextMeshProUGUI gameText)
    {
        int movePicked = Random.Range(0, 4);
        Debug.Log("moved picked " + movePicked);

        if (movePicked == 3)
        {
            if (enemies[charID].currHealth < enemies[charID].maxHealth && enemies[charID].currHealth != 0)
                Heal(enemyHUD[charID], enemies[charID], gameText);
        }
        else
            Attack(enemies, players, charID, playerHUD, gameText);
    }

    /*  Takes in everything it needs to deal damage to the opponent.    */
    private void Attack(List<CharacterStatus> enemies, List<CharacterStatus> players,
                           int charID, List<StatusHUD> playerHUD, TextMeshProUGUI gameText)
    {
        int playerPicked = Random.Range(0, players.Count);  //randomly chooses a player to attack

        gameText.text = enemies[charID].charName + " attacked " + players[playerPicked].charName + "!";

        playerHUD[playerPicked].SetHP(players[playerPicked], enemies[charID].strength, false);
    }

    /*  Takes in everything it needs to heal this unit.  */
    private void Heal(StatusHUD enemyHUD, CharacterStatus thisEnemy, TextMeshProUGUI gameText)
    {
        gameText.text = thisEnemy.charName + " healed for " + thisEnemy.strength + "!";
        enemyHUD.SetHP(thisEnemy, thisEnemy.strength * -1, true);
    }
}
