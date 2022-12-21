using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Attacks : MonoBehaviour
{
    //the combat manager
    public CombatManager cm;

    //HOLDS BOTH PARTIES AND THE CHARACTER WHOSE TURN IT CURRENTLY IS
    [Header("Characters")]
    public Party enemyParty;
    public Party playerParty;
    private CharacterStatus character;
    //public GameObject playerLoc;

    //THE BATTLE UI
    [Header("UI")]
    //public Button attackBtn;
    public GameObject playerUI; //this character's UI
    public TextMeshProUGUI gameText;    //the text that tells what move each character does
    public List<Button> btnList = new List<Button>();   //the list of buttons when choosing an enemy
    public List<Button> playerBtnList = new List<Button>(); //the list of buttons when choosing a ally
    public ScrollRect scrollArea;   //the area where all the moves appear
    public Button moveBtns; //the prefab of buttons for the moves

    //THE LISTS OF HUDS FOR BOTH PARTIES
    [Header("StatusHUD")]
    public List<StatusHUD> enemyHUDs = new List<StatusHUD>();
    public List<StatusHUD> playerHUDs = new List<StatusHUD>();

    private bool btnPressed = false;    //if the button was pressed
    //private Moves tempMove;

    /* THIS FUNCTION IS CALLED WHEN A CHARACTER'S TURN STARTS 
     * IT ALSO CREATES BUTTONS FOR EACH MOVE THE CHARACTER KNOWS */
    public void ThisTurn(CharacterStatus tempChar)
    {
        btnPressed = false;

        // lowers this character's UI to indicate whose turn it is
        ChangeUIPosition(true);

        Debug.Log("in player attack");
        character = tempChar;

        // creates all the buttons for the moves
        for (int i=0; i<character.charMoves.Count; i++)
        {
            if(character.level >= character.charMoves[i].lvlRequired)
            {
                if(character.currAbilityPts >= character.charMoves[i].apCost)
                {
                    Button tempBtn = Instantiate(moveBtns, scrollArea.content);
                    tempBtn.GetComponentInChildren<TextMeshProUGUI>().text = character.charMoves[i].moveName;
                    //tempMove = character.charMoves[i];

                    SetMoveBtn(character.charMoves[i], tempBtn);
                }
            }
        }

        scrollArea.gameObject.SetActive(true);
    }

    /* IF A MOVE THE PLAYER CHOSE IS A SINGLE TARGET ATTACK, 
     * THEN THE ENEMY CHOICE BUTTONS APPEAR SO THE PLAYER CAN CHOOSE AN ENEMY */
    public void OnAttackBtnPressed()
    {
        DestroyChildren();  // destroys the move buttons so extras don't appear on another character's turn

        Debug.Log("attack btn clicked");

        gameText.text = "Choose an enemy."; // changes the game text

        // shows the choice buttons of all the enemies
        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
        {
            if (enemyParty.partyMembers[i].currHealth > 0)
                btnList[i].gameObject.SetActive(true);
        }

        //attackBtn.gameObject.SetActive(false);
        scrollArea.gameObject.SetActive(false); //move list disappears

        //raises this characters ui back to its original place 
        ChangeUIPosition(false);

        btnPressed = true;
    }
    
    /* IF THE MOVE THE PLAYER SELECTS IS A SINGLE TARGET HEAL,
     * THEN THE PLAYER CHOICE BUTTONS APPEAR 
     * SO THE PLAYER CAN SELECT A PARTY MEMBER */
    public void OnHealBtnPressed()
    {
        DestroyChildren();

        Debug.Log("heal btn clicked");

        gameText.text = "Choose an party member.";

        // showing the player choice buttons
        for (int i = 0; i < playerParty.partyMembers.Count; i++)
        {
            if(playerParty.partyMembers[i].currHealth > 0)
            {
                playerBtnList[i].gameObject.SetActive(true);
            }
        }

        //attackBtn.gameObject.SetActive(false);
        scrollArea.gameObject.SetActive(false);

        ChangeUIPosition(false); // raises the this character's UI

        btnPressed = true;
    }

    /* IF THE MOVE SELECTED IS A MULTI TARGET ATTACK, ALL THE ENEMIES TAKE DAMAGE */
    public void OnAttackAllPressed(int amount)
    {
        DestroyChildren();

        gameText.text = character.charName + " attacked every enemy!";

        // damages every character in the enemy party
        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
        {
            int dmg_amount = character.strength - enemyParty.partyMembers[i].defense;

            if(dmg_amount < 1)
                dmg_amount = 1;

            enemyHUDs[i].SetHP(enemyParty.partyMembers[i], dmg_amount, false);
        }

        // decreses this characters AP
        playerUI.GetComponent<StatusHUD>().SetAP(character, amount);

        //attackBtn.gameObject.SetActive(false);
        scrollArea.gameObject.SetActive(false);

        ChangeUIPosition(false);

        btnPressed = true;

        cm.GetComponent<CombatManager>().NextFunction();    // moves to the next turn
    }
    
    /* IF THE SELECTED MOVE IS A MULTI TARGET HEAL,, ALL THE PARTY MEMBERS ARE HEALED */
    public void OnHealAllPressed(int amount)
    {
        DestroyChildren();

        gameText.text = character.charName + " healed the team!";

        // heals all characters in the player party
        for (int i = 0; i < playerParty.partyMembers.Count; i++)
        {
            playerHUDs[i].SetHP(character, character.strength, true);
        }

        // lowers this character's AP
        playerUI.GetComponent<StatusHUD>().SetAP(character, amount);

        //attackBtn.gameObject.SetActive(false);
        scrollArea.gameObject.SetActive(false);

        ChangeUIPosition(false);

        btnPressed = true;

        cm.GetComponent<CombatManager>().NextFunction();    // moves to the next turn
    }

    /* WHEN THE PLAYER SELECTS AN ENEMY TO DAMAGE WITH A 
     * SINGLE TARGET ATTACK, THAT ENEMY IS THEN DAMAGED */
    public void OnChoiceBtnPressed(int btnID)
    {
        Debug.Log("choice btn");

        scrollArea.gameObject.SetActive(false); // move list is deactivated

        //after a button was selected all the buttons deactivate
        for (int i = 0; i < enemyParty.partyMembers.Count; i++)
            btnList[i].gameObject.SetActive(false);

        // damages the selected enemy
        int dmg_amount = character.strength - enemyParty.partyMembers[btnID - 1].defense;

        if (dmg_amount < 1)
            dmg_amount = 1;

        enemyHUDs[btnID - 1].SetHP(enemyParty.partyMembers[btnID - 1], dmg_amount, false);

        //attackBtn.gameObject.SetActive(false);

        StartCoroutine(GameTextChange(enemyParty.partyMembers[btnID - 1], false));
    }
    
    public void OnHealChoiceBtnPressed(int btnID)
    {
        Debug.Log("heal choice btn");

        scrollArea.gameObject.SetActive(false);

        for (int i = 0; i < playerParty.partyMembers.Count; i++)
        {
            playerBtnList[i].gameObject.SetActive(false);
        }

        playerHUDs[btnID].SetHP(playerParty.partyMembers[btnID], character.strength, true);

        //attackBtn.gameObject.SetActive(false);

        StartCoroutine(GameTextChange(playerParty.partyMembers[btnID], true));
    }

    /* TEXT THAT TELLS THE PLAYER WHAT HAPPENED EACH TURN */
    IEnumerator GameTextChange(CharacterStatus enemy, bool isHeal)
    {
        // if it was an attack
        if(!isHeal)
            gameText.text = character.charName + " attacked " + enemy.charName + ".";
        else if(isHeal)
            gameText.text = character.charName + " healed for " + character.strength + ".";

        scrollArea.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);    // waits a second before disappearing

        cm.GetComponent<CombatManager>().NextFunction();    // moves on with the combat
    }

    /* WHEN THE BUTTONS ARE CREATED ON THE PLAYER'S TURN, 
     * IT SYPHONS THROUGH THE KNOWN MOVES AND CONNECTS 
     * THE BUTTONS WITH THE CORRESPONDING FUNCTIONS */
    private void SetMoveBtn(Moves move, Button tempBtn)
    {
        // checks the enum value given to the move
        if (move.moveType == moveType.DmgOne)
            tempBtn.onClick.AddListener(() => OnAttackBtnPressed());
        else if (move.moveType == moveType.DmgAll)
            tempBtn.onClick.AddListener(() => OnAttackAllPressed(move.apCost));
        else if (move.moveType == moveType.HealAll)
            tempBtn.onClick.AddListener(() => OnHealAllPressed(move.apCost));
        else if (move.moveType == moveType.HealOne)
            tempBtn.onClick.AddListener(() => OnHealBtnPressed());
        else // if they weren't any of the types listed above, it prints out the type in the console
            Debug.Log(move.moveType);   
    }

    /* DESTROYS ALL THE MOVE BUTTONS SO THEY DON'T APPEAR IN THE NEXT CHARACTER'S TURN */
    private void DestroyChildren()
    {
        // makes an array of all the buttons to destroy
        GameObject[] btnsToDestroy = GameObject.FindGameObjectsWithTag("ButtonsToDestroy");

        // then cycles through them destroying each one
        foreach(GameObject target in btnsToDestroy)
        {
            Destroy(target);
        }
    }

    /* CHANGES THE POSITION OF THE CHARACTER'S UI TO INDACATE WHOSE TURN IT IS */
    private void ChangeUIPosition(bool isDown)
    {
        if (isDown)
        {
            // lowers the UI
            playerUI.transform.position =
                new Vector3(playerUI.transform.position.x, playerUI.transform.position.y - 25, playerUI.transform.position.x);
        }
        else
        {
            // raises the UI
            playerUI.transform.position =
                new Vector3(playerUI.transform.position.x, playerUI.transform.position.y + 25, playerUI.transform.position.x);
        }
    }
}
