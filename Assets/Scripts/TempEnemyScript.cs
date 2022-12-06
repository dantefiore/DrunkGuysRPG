using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyScript : MonoBehaviour
{
    public int health = 3;

    public void DoDamage(int amount)
    {
        health -= amount;
    }
}
