using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotHandler : MonoBehaviour
{
    public float DestructionTime;

    private void Start()
    {
        Destroy(gameObject, DestructionTime);
    }
}
