using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPreviewObserver : MonoBehaviour
{
    [SerializeField] PlayerConfigurationHandler _playerConfigurationHandler;
    
    private void OnEnable()
    {
        PlayerConfigurationHandler.onPlayerReady += PreviewPlayer;
    }
    private void OnDisable()
    {
        PlayerConfigurationHandler.onPlayerReady -= PreviewPlayer;
    }

    public void PreviewPlayer(PlayerConfigurationHandler playerConfig, bool ready)
    {

        
    }
}
