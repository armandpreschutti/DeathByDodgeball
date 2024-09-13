using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageAreaHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ball")
        {
            Destroy(collision.gameObject);  
        }
    }
}
