using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        GameManager.GetInstance().onTransitionStart -= Test;
    }
    public void Start()
    {
        GameManager.GetInstance().onTransitionStart += Test;
    }
    public void Test()
    {
        
    }
}
