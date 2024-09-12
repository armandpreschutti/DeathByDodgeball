using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PawnAimingController : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;

    private void Awake()
    {
        playerStateMachine= GetComponentInParent<PlayerStateMachine>();
    }
    // Update is called once per frame
    void Update()
    {
        bool flipped;
        flipped = transform.position.x > 0f ? true : false;
        if (playerStateMachine != null && playerStateMachine.IsAiming)
        {
            if (flipped)
            {
                AimRightPosition.SetActive(false);
                AimLeftPosition.SetActive(true);
            }
            else
            {
                AimRightPosition.SetActive(true);
                AimLeftPosition.SetActive(false);
            }
        }
        else
        {
            AimRightPosition.SetActive(false);
            AimLeftPosition.SetActive(false);
        }
       
      
    }
}
