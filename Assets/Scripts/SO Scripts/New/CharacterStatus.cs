using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health", order = 1)]
public class CharacterStatus : ScriptableObject
{
    /* 
     * HOLDS ALL THE STATS, MOVES, AND THE PREFAB OF THE CHARACTER 
     * AS WELL AS WHERE THE PLAYER WAS STANDING LAST IN THE OVERWORLD
     * 
     * LASTLY IT HOLDS WHAT THIS CHARACTER'S LEVEL 1 STATS 
     * IF WE NEED TO RESET THE CHARACTER FOR WHATEVER REASON */

    [Header("General")]
    public string charName = "name";
    public int speed = 1;
    public int strength;
    public int defense;
    public GameObject characterGameObject;
    
    [Header("Health")]
    public float maxHealth = 10;
    public float currHealth = 10;


    [Header("Ability Pts")]
    public float maxAbilityPts = 10;
    public float currAbilityPts = 10;

    [Header("Levels")]
    public int level = 1;
    public int exp;

    [Header("Position")]
    public float[] position = new float[3];

    [Header("Player?")]
    public bool isPlayer = false;

    [Header("Turn Skipped?")]
    public bool isTurnSkipped = false;

    [Header("Moves")]
    public List<Moves> charMoves;

    [Header("Level One Stats")]
    public float lvlOneAbilityPts = 10;
    public float lvlOneHealth = 10;
    public int lvlOneSpeed = 1;
    public int lvlOneStrength;
    public int lvlOneDefense;
}
