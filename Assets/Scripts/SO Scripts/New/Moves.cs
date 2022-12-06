using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// WHICH TYPE OF MOVE IT IS
public enum moveType { HealOne, HealAll, DmgOne, DmgAll };

[CreateAssetMenu]
public class Moves : ScriptableObject
{
    public moveType moveType;
    public string moveName = "replace name";
    public int apCost = 0;  // if there is an AP cost
    public int lvlRequired = 0; //if there is a lvl requirement
    public bool skipTurn = false;   //if this move causes this character to skip its next turn
    //public Button thisBtn;
}
