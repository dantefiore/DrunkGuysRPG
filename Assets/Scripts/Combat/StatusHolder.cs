using System.Collections.Generic;
using UnityEngine;

public class StatusHolder : MonoBehaviour
{
    // literally just used so that GameObject can hold its CharacterStatus lol
    public CharacterStatus charStatus;

    int index;   // index of the defeated enemy in the saver lists
    [SerializeField] MapSaverSO saver;

    public void SetStatus()
    {
        if (charStatus.isPlayer && saver.fromBattle)
        {
            // the position where the player was before the bettle, and moves them there
            Vector3 position = new Vector3(charStatus.position[0], charStatus.position[1], charStatus.position[2]);
            this.gameObject.transform.position = new Vector3(position.x, 0.06899995f, position.z);

            // finds the index of the enemy in the saver lists
            index = saver.values.IndexOf(position);
            
            // removes the instance of the enemy in both lists
            saver.values.RemoveAt(index);
            saver.keys.RemoveAt(index);
        }
    }
}
