using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Battle { START, PLAYERTURN, ENEMYTURN, WIN, LOST }

public class CombatManager : MonoBehaviour
{
    /* THE GAME OBJECT OF THE PLAYER PARTY,
     * USED TO SPAWN THEM IN THE RIGHT LOCATION */
    [Header("Players")]
    private GameObject player1 = null;
    private GameObject player2 = null;
    private GameObject player3 = null;
    private GameObject player4 = null;
    private List<GameObject> players = new List<GameObject>();

    /* THE GAME OBJECT OF THE ENEMY PARTY,
     * USED TO SPAWN THEM IN THE RIGHT LOCATION */
    [Header("Enemies")]
    private GameObject enemy1 = null;
    private GameObject enemy2 = null;
    private GameObject enemy3 = null;
    private GameObject enemy4 = null;
    private List<GameObject> enemies = new List<GameObject>();

    /* THE LOCATIONS OF WHERE THE CHARACTERS STAND */
    [Header("Locations")]
    public List<Transform> playerLocs = new List<Transform>();
    public List<Transform> enemyLocs = new List<Transform>();

    /* THE PARTIES */
    [Header("Parties")]
    public Party playerParty;
    public Party enemyParty;

    /* THE UI'S FOR EACH CHARACTER */
    [Header("Player HUDs")]
    public StatusHUD playerHUD;
    public StatusHUD playerHUD2;
    public StatusHUD playerHUD3;
    public StatusHUD playerHUD4;
    private List<StatusHUD> playerStatusHUDs = new List<StatusHUD>();

    [Header("Enemy HUDs")]
    public StatusHUD enemyHUD1;
    public StatusHUD enemyHUD2;
    public StatusHUD enemyHUD3;
    public StatusHUD enemyHUD4;
    private List<StatusHUD> enemyStatusHUDs = new List<StatusHUD>();

    [Space] // THE TEXT TO TELL THE PLAYER WHAT HAPPENED
    public TextMeshProUGUI gameText;

    private Battle battleState; // WHAT TURN IT IS

    private bool hasClicked = true; // DID THE PLAYER HIT A BUTTON?

    /* THE TURN ORDER QUEUE (my new favorite data structure)
     * IT FOLLOWS THE RULES OF "FIFO" */
    private Queue<CharacterStatus> turnQueue = new Queue<CharacterStatus>();

    void Start()
    {
        battleState = Battle.START;

        // resets the enemies health
        for(int i=0; i<enemyParty.partyMembers.Count; i++)
            enemyParty.partyMembers[i].currHealth = enemyParty.partyMembers[i].maxHealth;

        SetCharacters();
        StartCoroutine(BeginBattle());
    }

    /* SETS EACH CHARACTER OBJECT WITH THE CORESPONDING PARTY MEMBER */
    void SetCharacters()
    {
        players.Add(player1);
        players.Add(player2);
        players.Add(player3);
        players.Add(player4);

        enemies.Add(enemy1);
        enemies.Add(enemy2);
        enemies.Add(enemy3);
        enemies.Add(enemy4);

        for(int i=0; i<playerParty.partyMembers.Count; i++)
            players[i] = playerParty.partyMembers[i].characterGameObject;

        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
            enemies[i] = enemyParty.partyMembers[i].characterGameObject;
    }

    /* IS CALLED AT THE BEGINING OF THE BATTLE, 
     * SETS THE CHARACTER'S LOCATION AND CONNECTS THEIR UI'S */
    IEnumerator BeginBattle()
    {
        //attackBtn.gameObject.SetActive(false);

        // spawn enemies on the platforms
        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
        {
            enemies[i] = Instantiate(enemyParty.partyMembers[i].characterGameObject, enemyLocs[i]); enemies[i].SetActive(true);
        }

        // spawn players on the platforms
        for (int i = 0; i < playerParty.partyMembers.Count; i++)
        {
            players[i] = Instantiate(playerParty.partyMembers[i].characterGameObject, playerLocs[i]); players[i].SetActive(true);
        }

        playerStatusHUDs.Add(playerHUD);
        playerStatusHUDs.Add(playerHUD2);
        playerStatusHUDs.Add(playerHUD3);
        playerStatusHUDs.Add(playerHUD4);

        // set the player characters stats in HUD displays
        for (int i=0; i < playerParty.partyMembers.Count; i++)
        {   
            playerStatusHUDs[i].SetStatusHUD(playerParty.partyMembers[i]);
            playerStatusHUDs[i].gameObject.SetActive(true);
        }

        enemyStatusHUDs.Add(enemyHUD1);
        enemyStatusHUDs.Add(enemyHUD2);
        enemyStatusHUDs.Add(enemyHUD3);
        enemyStatusHUDs.Add(enemyHUD4);

        // sets the enemy character stats in the HUD displays
        for (int j = 0; j < enemyParty.partyMembers.Count; j++)
        {   
            enemyStatusHUDs[j].SetStatusHUD(enemyParty.partyMembers[j]);
            enemyStatusHUDs[j].gameObject.SetActive(true);
        }

        //enemyHUD1.SetStatusHUD(enemyParty.partyMembers[0]);
        //enemyHUD2.SetStatusHUD(enemyParty.partyMembers[1]);

        yield return new WaitForSeconds(1);

        SetTurnOrder();
    }

    /* SETS THE TURN ORDER BY COMPARING ALL THE CHARACTERS SPEED STAT */
    void SetTurnOrder()
    {
        List<int> turnOrder = new List<int>();

        // adds all the character's speed to the turnOrder list
        for (int i = 0; i < playerParty.partyMembers.Count; i++)
            turnOrder.Add(playerParty.partyMembers[i].speed);

        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
            turnOrder.Add(enemyParty.partyMembers[i].speed);

        List<CharacterStatus> characters = new List<CharacterStatus>();

        // adds all the characters to a list
        for (int i = 0; i < playerParty.partyMembers.Count; i++)
            characters.Add(playerParty.partyMembers[i]);

        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
            characters.Add(enemyParty.partyMembers[i]);

        turnOrder.Sort();   // sorts the turnOrder from lowest to highest
        turnOrder.Reverse();    // reverses that list to get highest to lowest

        // compares both lists and adds the character's to a queue
        for (int i = 0; i < turnOrder.Count; i++)
        {
            for (int ii = 0; ii < characters.Count; ii++)
            {
                if (characters[ii].speed == turnOrder[i])
                {
                    // checks if a character was already added
                    if (!turnQueue.Contains(characters[ii]))
                    {
                        Debug.Log(characters[ii].charName + " was added to the queue");
                        turnQueue.Enqueue(characters[ii]);
                        break;
                    }

                }
                
            }
        }

        Debug.Log(turnQueue.Count); // checks if the correct amount was added
        NextTurn();
    }

    /* CHECKS WHOSE TURN IT IS NEXT AND CALLS THE CORESPONDING FUNCTIONS  */
    void NextTurn()
    {
        // checks if the queue is empty
        if (turnQueue.Count > 0)
        {
            CharacterStatus tempCharacter = turnQueue.Dequeue();
            
            if (tempCharacter.isPlayer && !tempCharacter.isTurnSkipped)
            {
                // if the character is the player and their turn isn't skipped

                battleState = Battle.PLAYERTURN;
                Debug.Log("player");
                StartCoroutine(PlayerTurn(tempCharacter));
            }
            else if(!tempCharacter.isPlayer && !tempCharacter.isTurnSkipped)
            {
                // if it isn't the players turn and their turn isn't skipped
                battleState = Battle.ENEMYTURN;
                Debug.Log("enemy");
                StartCoroutine(EnemyTurn(tempCharacter));
            }
            else
            {
                // if the characters turn is skipped, it makes the bool false so the next one isn't skipped
                tempCharacter.isTurnSkipped = false;
                gameText.text = tempCharacter.charName + " turn was skipped.";
                
                NextTurn();
            }
        }
        else
            SetTurnOrder();
    }

    /* IF THE CHARACTER IS ALIVE, THIS CALLS ATTACK SCRIPT OF THAT CHARACTER */
    IEnumerator PlayerTurn(CharacterStatus tempChar)
    {
        if (tempChar.currHealth > 0)
        {
            int characterID = 0;

            // display message
            gameText.text = "It is " + tempChar.charName + "'s turn.";

            // starting player's turn
            yield return new WaitForSeconds(0.5f);

            hasClicked = false;
            Debug.Log("in player turn");

            // gets the character's position in the party so it can call the correct Attack Script
            for (int i = 0; i < playerParty.partyMembers.Count; i++)
                if (tempChar.charName == playerParty.partyMembers[i].charName)
                    characterID = i;

            playerLocs[characterID].GetComponent<Attacks>().ThisTurn(tempChar);

            //attackBtn.gameObject.SetActive(true);
        }
        else
        {
            DetermineEnd();
        }
    }

    /* CALLS THE COROUTINE DETERMINEEND() */
    public void NextFunction()
    {
        gameText.text = "";
        StartCoroutine(DetermineEnd());
    }

    /* CHECKS IF ALL OF ONE PARTY WAS DEFEATED */
    public IEnumerator DetermineEnd()
    {
        Debug.Log("next function");
        int deathCount = 0;
        int enemyDeathCount = 0;

        // checks how many enemies are dead
        for(int i=0; i<enemyParty.partyMembers.Count; i++)
        {
            if (enemyParty.partyMembers[i].currHealth <= 0)
            {
                enemyLocs[i].gameObject.SetActive(false);
                enemyStatusHUDs[i].gameObject.SetActive(false);

                enemyDeathCount++;
            }
        }
        
        // checks how many player characters are dead
        for(int i=0; i<playerParty.partyMembers.Count; i++)
        {
            if (playerParty.partyMembers[i].currHealth <= 0)
            {
                playerLocs[i].gameObject.SetActive(false);
                playerStatusHUDs[i].gameObject.SetActive(false);

                deathCount++;
            }
        }

        /* if the death total is equal to the party's size, 
         * the battle ends, otherwise it moves on to the next turn */
        if (enemyDeathCount == enemyParty.partyMembers.Count)
        {
            battleState = Battle.WIN;
            yield return StartCoroutine(EndBattle());
        }
        else if(deathCount == playerParty.partyMembers.Count)
        {
            battleState = Battle.LOST;
            yield return StartCoroutine(EndBattle());
        }
        else
            NextTurn();
    }

    /* CALLS THE ENEMY ATTACK SCRIPT */
    IEnumerator EnemyTurn(CharacterStatus tempChar)
    {
        if(tempChar.currHealth > 0)
        {
            yield return new WaitForSeconds(1);

            // checks the character's to make sure its calling the correct EnemyAttacks Script
            int characterID = 0;
            for (int i = 0; i < enemyParty.partyMembers.Count; i++)
                if (tempChar.charName == enemyParty.partyMembers[i].charName)
                    characterID = i;

            // calls the script and passes in all the info needed to complete its task
            enemies[characterID].GetComponent<EnemyAttacks>().TakeTurn(enemyParty.partyMembers, playerParty.partyMembers, characterID,
                                                                              playerStatusHUDs, enemyStatusHUDs, gameText);

            yield return new WaitForSeconds(1);

            StartCoroutine(DetermineEnd());
        }
        else
        {
            NextTurn();
        }
    }

    /* CHECKS IF THE BATTLE IS WON OR LOST, DOES END OF BATTLE CALCULATIONS, THEN LEAVES THE SCENE */
    IEnumerator EndBattle()
    {
        if (battleState == Battle.WIN)
        {
            // changes the text to the tell the player they won and stays for 2 seconds
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Player wins");

            gameText.text = "You Win!";
            yield return new WaitForSeconds(2f);

            int gainedExp = 0;

            // calculates how much EXP the party gets
            for (int i = 0; i < enemyParty.partyMembers.Count; i++)
            { 
                gainedExp += enemyParty.partyMembers[i].exp;
            }

            /* gives EXP to the characters who are alive, 
             * and changes the text to tell the player who gained how much */
            for (int j = 0; j < playerParty.partyMembers.Count; j++)
            {
                if(playerParty.partyMembers[j].currHealth > 0)
                {
                    playerParty.partyMembers[j].exp += gainedExp;
                    players[j].GetComponent<LevelUp>().CheckExp();
                }

                gameText.text = playerParty.partyMembers[j].name + " gained " + gainedExp + " exp!";
                yield return new WaitForSeconds(2f);
            }

            /* Loads the SampleScene
             * -- LATER CHANGE IT TO THE SCENE THE PLAYER WAS IN PREVIOUSLY AND WHERE THEY WERE -- */
            LevelLoader.instance.LoadLevel("SampleScene");
        }
        else if (battleState == Battle.LOST)
        {
            gameText.text = "You Lose.";

            yield return new WaitForSeconds(0.5f);
            Debug.Log("Player lost");
            //later load a game over screen
            LevelLoader.instance.LoadLevel("SampleScene");

            /* RIGHT NOW THIS JUST SEND THE PLAYER BACK TO THE SAMPLESCENE, 
             * LATER WE WILL NEED TO CHANGE IT TO A LOST SCREEN OR 
             * BACK AT THE NEAREST HEALING LOCATION */
        }
    }
}