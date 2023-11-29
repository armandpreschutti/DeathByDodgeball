using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameManagerCameraObserver : MonoBehaviour
{
    private void OnEnable()
    {
        BallStateMachine.OnExplosion += CameraShake;
    }
    private void OnDisable()
    {
        BallStateMachine.OnExplosion -= CameraShake;
    }
    public void CameraShake()
    {
        Vector3 shakeDirection = new Vector3(0, .25f, 0);
        transform.DOShakePosition(1f, shakeDirection, 10, 0 , false, true, ShakeRandomnessMode.Harmonic);

    }
}
