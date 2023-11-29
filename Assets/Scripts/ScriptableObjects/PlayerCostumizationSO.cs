using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "DBDB/Player Costumization")]
public class PlayerCostumizationSO : ScriptableObject
{
    public List<AnimatorOverrideController> team1Skins;
    public List<AnimatorOverrideController> team2Skins;
    public List<Sprite> team1Sprites;
    public List<Sprite> team2Sprites;

    public void GetRandomSkin(PlayerManager playerManager)
    {
        int randomIndex = Random.Range(0, team1Skins.Count);
        playerManager.SkinId = randomIndex;
        if (playerManager.TeamId == 1)
        {
            playerManager.anim.runtimeAnimatorController = team1Skins[randomIndex];
            playerManager.GetComponent<SpriteRenderer>().sprite = team1Sprites[randomIndex];

        }
        else if(playerManager.TeamId == 2)
        {
            playerManager.anim.runtimeAnimatorController = team2Skins[randomIndex];
            playerManager.GetComponent<SpriteRenderer>().sprite = team2Sprites[randomIndex];
        }
        else
        {
            return;
        }
    }
    public void TogglePlayerSkin(PlayerManager playerManager)
    {
        if (playerManager.TeamId == 1)
        {
            if(playerManager.SkinId >= 7)
            {
                playerManager.SkinId = 0;
            }
            else
            {
                playerManager.SkinId++;

            }
            playerManager.anim.runtimeAnimatorController = team1Skins[playerManager.SkinId];
            playerManager.GetComponent<SpriteRenderer>().sprite = team1Sprites[playerManager.SkinId];
        }
        else if (playerManager.TeamId == 2)
        {
            if (playerManager.SkinId >= 7)
            {
                playerManager.SkinId = 0;
            }
            else
            {
                playerManager.SkinId++;

            }
            playerManager.anim.runtimeAnimatorController = team2Skins[playerManager.SkinId];
            playerManager.GetComponent<SpriteRenderer>().sprite = team2Sprites[playerManager.SkinId];
        }
        else
        {
            return;
        }
    }
    public void ToggleTeamSkin(PlayerManager playerManager)
    {
        if (playerManager.TeamId == 1)
        {
            playerManager.anim.runtimeAnimatorController = team1Skins[playerManager.SkinId];
            playerManager.GetComponent<SpriteRenderer>().sprite = team1Sprites[playerManager.SkinId];
        }
        else if (playerManager.TeamId == 2)
        {
            playerManager.anim.runtimeAnimatorController = team2Skins[playerManager.SkinId];
            playerManager.GetComponent<SpriteRenderer>().sprite = team2Sprites[playerManager.SkinId];
        }
        else
        {
            Debug.Log("Something went wrong trying to increase skin index");
            return;
        }
    }
}
