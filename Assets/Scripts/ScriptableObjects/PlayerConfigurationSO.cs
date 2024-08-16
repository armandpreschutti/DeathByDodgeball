using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DBDB/Configurations/PlayerConfiguration")]
public class PlayerConfigurationSO : ScriptableObject
{
    public int playerId;
    public int teamId;
    public int skinID;

    public void SetTeam(int slot)
    {
        Debug.Log("SO Function was called correctly");
        if (slot == 1 || slot == 2)
        {
            teamId = 1;
        }
        else if(slot == 3 || slot == 4)
        {
            teamId = 2;
        }
    }
}
