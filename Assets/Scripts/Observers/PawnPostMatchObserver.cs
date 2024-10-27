using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawnPostMatchObserver : MonoBehaviour
{
    public PlayerConfigurationSO pawnConfig;
    public Image pawnPreview;
    public TextMeshProUGUI pawnTag;
    public Image pawnPanel;
    public PreviewSkinsSO blueSkins;
    public PreviewSkinsSO redSkins;

    // Start is called before the first frame update
    void Start()
    {
        SetPawnPreview();
        SetPawnTag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPawnPreview()
    {
        switch(pawnConfig.teamId)
        {
            case 1:
                pawnPreview.sprite = blueSkins.skins[pawnConfig.skinID];
                break;

            case 2:
                pawnPreview.sprite = redSkins.skins[pawnConfig.skinID];
                break;
        }
    }

    public void SetPawnTag()
    {
        
        pawnTag.text = pawnConfig.playerName;
    }

}
