using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MatchInstanceManager : MonoBehaviour
{

    public static Action onMatchInstanceLoaded;
    public GameObject[] blueTeamConfigurations;
    public GameObject[] redTeamConfigurations;
    public GameObject playablePawn;

    public static Action onInitializeMatchInstance;
    public static Action onStartMatch;
    public static Action onDisablePawnControl;
    public static Action onEnablePawnControl;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ConfigureMatchInstance;
        PawnManager.onPlayerLoaded += AddPlayerToMatch;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ConfigureMatchInstance;
        PawnManager.onPlayerLoaded -= AddPlayerToMatch;
    }

    public void ConfigureMatchInstance(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MatchInstance" && GameObject.Find("PlayerSelectionManager").GetComponent<PlayerSelectionManager>() != null)
        {
            PlayerSelectionManager playerSelectionManager;
            playerSelectionManager = GameObject.Find("PlayerSelectionManager").GetComponent<PlayerSelectionManager>();
            for(int i = 0; i < playerSelectionManager.playerConfigurations.Length; i++)
            {
                if (playerSelectionManager.playerConfigurations[i]!= null)
                {
                    CreatePlayerInstance(playerSelectionManager.playerConfigurations[i]);
                }
            }
            Destroy(playerSelectionManager.gameObject);
            onInitializeMatchInstance?.Invoke();
        }
    }

    public void CreatePlayerInstance(PlayerConfigurationSO so)
    {
        GameObject pawn = playablePawn;
        PawnManager pawnManager = pawn.GetComponent<PawnManager>();
        pawn.name = $"PlayablePawn{so.playerId}";
        pawnManager.slotId = so.slotId;
        pawnManager.skinId = so.skinID;
        pawnManager.playerId = so.playerId;
        pawnManager.teamId = so.teamId;
        Instantiate(pawn, transform.position, Quaternion.identity);
    }

    public void AddPlayerToMatch(int slotId, GameObject pawn)
    {
        switch (slotId)
        {
            case 1:
                blueTeamConfigurations[0] = pawn;
                break;
            case 2:
                blueTeamConfigurations[1] = pawn;
                break;
            case 3:
                redTeamConfigurations[0] = pawn;
                break;
            case 4:
                redTeamConfigurations[1] = pawn;
                break;

        }
    }

    public void StartMatch()
    {
        onStartMatch?.Invoke();
    }

    public void EnablePawnControl()
    {
        onEnablePawnControl?.Invoke();
    }
    public void DisablePawnControl()
    {
        onDisablePawnControl?.Invoke();
    }
}
