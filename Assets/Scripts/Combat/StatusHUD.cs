using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusHUD : MonoBehaviour
{
    // THE TEXT FOR NAME AND LEVEL
    [Header("Title")]
    public TextMeshProUGUI charName;
    public TextMeshProUGUI level;

    // BAR AND TEXT FOR HEALTH
    [Header("HP")]
    public Image hpBar;
    public TextMeshProUGUI hpValue;

    //BAR AND TEXT FOR ABILITY POINTS
    [Header("AP")]
    public Image abilityBar;
    public TextMeshProUGUI abilityValue;

    /* SETS THE HEALTH, LEVEL, HEALTH, AND AP */
    public void SetStatusHUD(CharacterStatus status)
    {
        charName.text = status.charName;
        level.text = status.level.ToString();

        float currentHealth = status.currHealth * (10 / status.maxHealth);
        float currentMana = status.currAbilityPts * (10 / status.maxAbilityPts);

        hpBar.fillAmount = currentHealth / 10;
        hpValue.SetText(status.currHealth + "/" + status.maxHealth);

        if (status.isPlayer)
        {
            abilityBar.fillAmount = currentMana / 10;
            abilityValue.SetText(status.currAbilityPts + "/" + status.maxAbilityPts);
        }
    }

    /*public void SetHP(CharacterStatus status, float dmg)
    {
        GraduallySetStatusBar(status, dmg, false, 1, 0.05f);
    }*/
    
    /* CALLED WHEN AP NEEDS TO BE DECREASED OR INCREASED */
    public void SetAP(CharacterStatus status, float amount)
    {
        //StartCoroutine(ChangeAPBar(status, amount, false, 1, 0.05f));

        // decreases the AP in the CharacterStatus
        status.currAbilityPts -= amount;

        // changes how much the bar should be filled
        abilityBar.fillAmount -= amount / status.maxAbilityPts;

        if (status.currAbilityPts > 0)
        {
            // if the character is left with more than 0 AP,
            // then the text is changed to show the correct amount
            abilityValue.SetText(status.currAbilityPts + "/" + status.maxAbilityPts);
        }
        else
        {
            // if the character has 0 or less AP, then the text is changed to 0
            hpValue.SetText("0/" + status.maxAbilityPts);
            abilityBar.fillAmount = 0;
            status.currAbilityPts = 0;
        }
    }

    /* IS CALLED WHEN THE HEALTH IS INCREASED OR DECREASED, 
     * THEN CHANGES THE UI TO MATCH */
    //used to be called GraduallySetStatusBar
    public void SetHP(CharacterStatus status, float amount, bool isIncrease)
    {
        int fillTimes = 1;
        //float filldelay = 0.05f;

        float percentage = 1 / (float)fillTimes;

        if (isIncrease)
        {
            // increases the health and if its over the max health, it is set to the max health
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                status.currHealth += _fAmount;
                hpBar.fillAmount += _dAmount;

                if (status.currHealth > status.maxHealth)
                {
                    status.currHealth = status.maxHealth;
                }
                 
                hpValue.SetText(status.currHealth + "/" + status.maxHealth);

                //yield return new WaitForSeconds(fillDelay);
            }
        }
        else
        {
            // decreases the health and if its lower than 0, it is set back to 0
            for (int fillStep = 0; fillStep < fillTimes; fillStep++)
            {
                float _fAmount = amount * percentage;
                float _dAmount = _fAmount / status.maxHealth;
                status.currHealth -= _fAmount;
                hpBar.fillAmount -= _dAmount;
                if (status.currHealth >= 0)
                    hpValue.SetText(status.currHealth + "/" + status.maxHealth);
                else
                    hpValue.SetText("0/" + status.maxHealth);

                //yield return new WaitForSeconds(fillDelay);
            }
        }
    }
}