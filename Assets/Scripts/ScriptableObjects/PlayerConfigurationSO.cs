using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DBDB/Configurations/PlayerConfiguration")]
public class PlayerConfigurationSO : ScriptableObject
{
    public int playerId;
    public int teamId;
    public int skinID;

    private void OnValidate()
    {
        Debug.Log("SO Function was called correctly");
    }

    public void SetTeam(int slot)
    {
        if(slot == 1 || slot == 2)
        {
            teamId = 1;
        }
        else if(slot == 3 || slot == 4)
        {
            teamId = 2;
        }
    }
}
