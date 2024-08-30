using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSkinHandler : MonoBehaviour
{
    public PawnTeamSkinsSO pawnTeamSkinsSO;
    PawnManager pawnManager;

    private void Awake()
    {
        pawnManager = GetComponentInParent<PawnManager>();
    }

    private void Start()
    {
        if(pawnManager != null)
        {
            if (pawnManager.teamId == 1)
            {
                GetComponent<Animator>().runtimeAnimatorController = pawnTeamSkinsSO.blueTeamSkins[pawnManager.skinId];
            }
            else
            {
                GetComponent<Animator>().runtimeAnimatorController = pawnTeamSkinsSO.redTeamSkins[pawnManager.skinId];
            }
        }
        
    }
}
