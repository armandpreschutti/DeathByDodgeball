using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DBDB/Skins/PawnSkins")]
public class PawnTeamSkinsSO : ScriptableObject
{
    public AnimatorOverrideController[] blueTeamSkins;
    public AnimatorOverrideController[] redTeamSkins;
}
