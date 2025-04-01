using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class PawnHUDObserver : MonoBehaviour
{
    public int slotId;
    public TextMeshProUGUI pawnName;
    public GameObject eliminationPrompt;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public bool hasOwner;
    public Image border;
    public HealthSystem healthSystem;
    public Image playerIcon;
    public PreviewSkinsSO playerIcons;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        HealthSystem.onHealthInitialized += SetHealthSystemUI;
        HealthSystem.onPlayerDeath += RemoveLife;
        HealthSystem.onPlayerHealed += AddLife;
        HealthSystem.onPlayerElimination += SetEliminationPrompt;
    }

    private void OnDisable()
    {
        HealthSystem.onHealthInitialized += SetHealthSystemUI;
        HealthSystem.onPlayerDeath -= RemoveLife;
        HealthSystem.onPlayerHealed -= AddLife;
        HealthSystem.onPlayerElimination -= SetEliminationPrompt;
    }

    private void Start()
    {
        StartCoroutine(DisableUnowned());
    }

    public void SetHealthSystemUI(int slot, string newName, HealthSystem system)
    {
        if(slotId == slot)
        {
            pawnName.text = newName;
            hasOwner = true;
            healthSystem = system;
        }
    }

    public IEnumerator DisableUnowned()
    {
        yield return new WaitForSeconds(.25f);
        if(hasOwner)
        {
            border = GetComponent<Image>();
            border.color = healthSystem.GetComponent<PawnManager>().pawnColor;
            pawnName.color = healthSystem.GetComponent<PawnManager>().pawnColor;
            playerIcon.sprite = playerIcons.skins[healthSystem.GetComponent<PawnManager>().skinId];
        }
        else
        {
            Destroy(gameObject);

        }
    }

    public void RemoveLife(int slot, int lives)
    {
        if(slotId == slot)
        {
            switch(lives)
            {
                case 2:
                    heart3.gameObject.SetActive(false);
                    break;
                case 1:
                    heart2.gameObject.SetActive(false);
                    break;
                case 0:
                    heart1.gameObject.SetActive(false);

                    break;
            }
        }
    }

    public void AddLife(int slot, int lives)
    {
        if (slotId == slot)
        {
            switch (lives)
            {
                case 3:
                    heart3.gameObject.SetActive(true);

                    break;
                case 2:
                    heart2.gameObject.SetActive(true);
                    break;
                case 1:
                    heart1.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void SetEliminationPrompt(int slot)
    {
        if (slotId == slot)
        {
            eliminationPrompt.SetActive(true);
        }
    }
    
  
}
