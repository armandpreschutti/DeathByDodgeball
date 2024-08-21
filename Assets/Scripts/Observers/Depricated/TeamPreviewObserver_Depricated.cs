using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPreviewObserver : MonoBehaviour
{
    [SerializeField] PlayerConfigurationHandler_Depricated _playerConfigurationHandler;
    
    private void OnEnable()
    {
        PlayerConfigurationHandler_Depricated.onPlayerReady += PreviewPlayer;
    }
    private void OnDisable()
    {
        PlayerConfigurationHandler_Depricated.onPlayerReady -= PreviewPlayer;
    }

    public void PreviewPlayer(PlayerConfigurationHandler_Depricated playerConfig, bool ready)
    {

        
    }
}
