using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "DBDB/Configurations/PlayerConfiguration")]
public class PlayerConfigurationSO : ScriptableObject
{
    public int playerId;
    public int teamId;
    public int skinID;
    public int slotId;
    public string playerName;
    public Color playerColor;
    public int matchStatKills;
    public int matchStatHits;
    public int matchStatCatches;
    public int matchStatDodges;
    public int matchStatAttempts;
    public Image playerIcon;

    public void SetTeam()
    {
        if (slotId == 1 || slotId == 2)
        {
            teamId = 1;
        }
        else if(slotId == 3 || slotId == 4)
        {
            teamId = 2;
        }
    }

    public void ResetTeam()
    {
        teamId = 0;
    }

}
