using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private void Awake()
    {

    }
    private void OnEnable()
    {
        HealthSystem.onHealthInitialized += SetHealthSystemUI;
        HealthSystem.onPlayerDeath += RemoveLife;
        HealthSystem.onPlayerElimination += SetEliminationPrompt;
    }

    private void OnDisable()
    {
        HealthSystem.onHealthInitialized += SetHealthSystemUI;
        HealthSystem.onPlayerDeath -= RemoveLife;
        HealthSystem.onPlayerElimination -= SetEliminationPrompt;
    }

    private void Start()
    {
        StartCoroutine(DisableUnownded());
    }

    public void SetHealthSystemUI(int slot, string newName, HealthSystem system)
    {
        if(slotId == slot)
        {
            pawnName.text = newName;
            hasOwner = true;
        }
    }

    public IEnumerator DisableUnownded()
    {
        yield return new WaitForSeconds(.25f);
        if(!hasOwner)
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
                heart1.gameObject.SetActive(false);
                break;
            case 1:
                heart2.gameObject.SetActive(false);
                break;
            case 0:
                heart3.gameObject.SetActive(false);
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
