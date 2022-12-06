using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Party : ScriptableObject
{
    /* THESE ARE HOW BIG THE PARTY IS ALLOWED TO BE WHEN PARTY CHANGING IS ADDED 
     * THESE VALUES WILL BE USED, THERE ARE COMMENTED OUT RIGHT NOW TO GET RID OF A WARNING
    
    private int maxNumberInParty = 4;
    private int minNumberInParty = 1;
    
     */

    public List<CharacterStatus> partyMembers = new List<CharacterStatus>();
}
