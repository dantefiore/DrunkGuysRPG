using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    // holds the party and each location of the overworld

    [SerializeField] private Party party;
    [SerializeField] private List<SpriteRenderer> partyMembersLoc = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        SetParty();
    }

    // THIS IS TEMPORARY, IT WAS FOR TESTING TO
    // MAKE SURE CHAGNING THE SIZE OF THE PARTY
    // WILL AFFECT THE OVERWORLD PARTY
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetParty();
        }
    }

    void SetParty()
    {
        for(int j = 0; j < partyMembersLoc.Count; j++)
        {
            if(j < party.partyMembers.Count)
                partyMembersLoc[j].sprite = party.partyMembers[j].characterGameObject.GetComponent<SpriteRenderer>().sprite;
            else
                partyMembersLoc[j].sprite = null;
        }
    }
}
