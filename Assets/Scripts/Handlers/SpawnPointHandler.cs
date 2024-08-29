using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHandler : MonoBehaviour
{
    public int teamId;
    public int slotId;
    public GameObject pawnOwner;

    private void OnEnable()
    {
        MatchInstanceManager.onInitializeMatchInstance += PlacePawn;
    }
    private void OnDisable()
    {
        MatchInstanceManager.onInitializeMatchInstance -= PlacePawn;
    }
    public void Start()
    {

    }

    public void PlacePawn()
    {
        PawnManager[] avaialiblePawns = FindObjectsOfType<PawnManager>();
        for(int i = 0; i < avaialiblePawns.Length; i++)
        {
            if (avaialiblePawns[i].slotId == slotId)
            {
                pawnOwner = avaialiblePawns[i].gameObject;
                avaialiblePawns[i].transform.position = transform.position;
            }
        }

    }
}
