using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointHandler : MonoBehaviour
{
    public int teamId;
    public int spawnId;
    public PlayerManager _playerManager;

    private void OnEnable()
    {
        HealthHandler.OnRespawn += RespawnPlayerPosition;
    }
    private void OnDisable()
    {
        HealthHandler.OnRespawn -= RespawnPlayerPosition;
    }
    public void Start()
    {
        SetPlayerStartPoint();
    }
    public void RespawnPlayerPosition(GameObject gameObject)
    {
        if(gameObject.GetComponent<PlayerManager>() == _playerManager)
        {
            _playerManager.transform.position = this.transform.position;
        }
    }
    public void SetPlayerDirection()
    {
        bool flipped = transform.position.x > 0; 
        _playerManager.GetComponent<SpriteRenderer>().flipX= flipped;
    }
    public void SetPlayerStartPoint()
    {
        if(GameManager.GetInstance() != null)
        {
            if (teamId == 1)
            {
                if (spawnId == 1)
                {
                    if (GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[0] != null)
                    {
                        GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[0].transform.position = transform.position;
                        _playerManager = GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[0].GetComponent<PlayerManager>();
                        SetPlayerDirection();
                    }
                    else
                    {
                        Debug.LogWarning("Player T1P1 not found");
                    }

                }
                else if (spawnId == 2 && GameManager.GetInstance().GetComponent<   LocalMatchManager>().team1.Count == 2)
                {
                    if (GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[1] != null)
                    {
                        GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[1].transform.position = transform.position;
                        _playerManager = GameManager.GetInstance().GetComponent<LocalMatchManager>().team1[1].GetComponent<PlayerManager>();
                        SetPlayerDirection();
                    }
                    else
                    {
                        Debug.LogWarning("Player T1P2 not found");
                    }
                }
                else
                {
                    return;
                }

            }
            else if (teamId == 2)
            {
                if (spawnId == 1)
                {
                    if (GameManager.GetInstance().GetComponent<LocalMatchManager>().team2[0] != null)
                    {
                        GameManager.GetInstance().GetComponent<LocalMatchManager>().team2[0].transform.position = transform.position;
                        _playerManager = GameManager.GetInstance().GetComponent<LocalMatchManager>().team2[0].GetComponent<PlayerManager>();
                        SetPlayerDirection();
                    }
                    else
                    {
                        Debug.LogWarning("Player T2P1 not found");
                    }
                }
                else if (spawnId == 2 && GameManager.GetInstance().GetComponent<   LocalMatchManager>().team2.Count == 2)
                {
                    if (GameManager.GetInstance().GetComponent<LocalMatchManager>().team2[1] != null)
                    {
                        GameManager.GetInstance().PreMatchManager._team2[1].transform.position = transform.position;
                        _playerManager = GameManager.GetInstance().GetComponent<LocalMatchManager>().team2[1].GetComponent<PlayerManager>();
                        SetPlayerDirection();
                    }
                    else
                    {
                        Debug.LogWarning("Player T2P1 not found");
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                Debug.LogError("No Team found");
                return;
            }
        }
        else
        {
            Debug.Log("No Match Configuration Found");
            return;
        }        
    }
}
