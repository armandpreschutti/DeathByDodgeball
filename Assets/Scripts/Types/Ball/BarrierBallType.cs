using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBallType : BallManager
{
    public Color errorBuildColor;
    public GameObject normalBarrier;
    public GameObject SuperBarrier;
    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;
    public GameObject selfDestructVFX;
    bool flipped;

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);

        if (owner.IsNoBuildArea)
        {
            Instantiate(selfDestructVFX, transform.position, Quaternion.identity, null);
            Destroy(gameObject);

        }
        else
        {
            GameObject barrier;
            if (isSuperBall)
            {
                barrier = SuperBarrier;
                GameObject newBarrier = Instantiate(barrier, flipped ? AimLeftPosition.transform.position : AimRightPosition.transform.position, Quaternion.identity, null);
                newBarrier.GetComponent<BarrierHandler>().owningTeam = owningTeam;
            }
            else
            {
                barrier = normalBarrier;
                GameObject newBarrier = Instantiate(barrier, flipped ? AimLeftPosition.transform.position : AimRightPosition.transform.position, Quaternion.identity, null);
                newBarrier.GetComponent<BarrierHandler>().owningTeam = owningTeam;
            }
            Destroy(gameObject);
        }

    }

    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        
        flipped = transform.position.x > 0f ? true : false;
        if (owner != null && owner.IsAiming && !isBallActive)
        {
            if (flipped)
            {
                AimRightPosition.SetActive(false);
                AimLeftPosition.SetActive(true);
                if (owner.IsSuper)
                {
                    AimLeftPosition.GetComponent<SpriteRenderer>().color = owner.IsNoBuildArea ? errorBuildColor : superAimColor;
                    AimLeftPosition.transform.localScale = new Vector3(.8f, 2.75f, 1f);
                }
                else
                {
                    AimLeftPosition.GetComponent<SpriteRenderer>().color = owner.IsNoBuildArea ? errorBuildColor : normalAimColor;
                    AimLeftPosition.transform.localScale = new Vector3(.8f, 1.5f, 1f);
                }
            }
            else
            {
                AimRightPosition.SetActive(true);
                AimLeftPosition.SetActive(false);
                if (owner.IsSuper)
                {
                    AimRightPosition.GetComponent<SpriteRenderer>().color = owner.IsNoBuildArea ? errorBuildColor : superAimColor;
                    AimRightPosition.transform.localScale = new Vector3(.8f, 2.75f, 1f);
                }
                else
                {
                    AimRightPosition.GetComponent<SpriteRenderer>().color = owner.IsNoBuildArea ? errorBuildColor : normalAimColor;
                    AimRightPosition.transform.localScale = new Vector3(.8f, 1.5f, 1f);
                }
            }
        }
        else
        {
            AimRightPosition.SetActive(false);
            AimLeftPosition.SetActive(false);
        }
    }
}
