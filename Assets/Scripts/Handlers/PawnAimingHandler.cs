using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAimingHandler : MonoBehaviour
{
    //public Transform player; // Reference to the player

    public HomingBallType homingBallManager;
    public Transform mainTarget; // Closest target to the player
    public PlayerStateMachine playerStateMachine;
    private Collider2D[] objectsInArea = new Collider2D[10]; // Fixed size array to store objects in the area
    private int objectCount = 0; // Tracks the number of objects in the array



    // Called when an object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && objectCount < objectsInArea.Length)
        {
            if(collision.GetComponent<PawnManager>().teamId != homingBallManager.owningTeam)
            {
                objectsInArea[objectCount] = collision;
                objectCount++;
            }

        }
    }


    // Called when an object exits the trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < objectCount; i++)
        {
            if (objectsInArea[i] == collision)
            {
                // Shift the remaining elements left after removal
                for (int j = i; j < objectCount - 1; j++)
                {
                    objectsInArea[j] = objectsInArea[j + 1];
                }
                objectsInArea[objectCount - 1] = null; // Clear the last element
                objectCount--;
                break;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        mainTarget = GetClosestTargetToPlayerAndCenter(0.5f, .05f);
        if (objectCount > 0 && mainTarget != null)
        {
            homingBallManager.currentTarget = mainTarget.gameObject;
        }
        else
        {
            mainTarget = null;
            homingBallManager.currentTarget = null;
        }

    }

 
    private Transform GetClosestTargetToPlayerAndCenter(float weightY, float weightPlayer)
    {
        Transform closest = null;
        float minScore = Mathf.Infinity;

        for (int i = 0; i < objectCount; i++)
        {
            // Get the collider's center position
            Vector3 targetCenter = objectsInArea[i].bounds.center;

            // Calculate y-axis distance and player distance
            float distanceY = Mathf.Abs(transform.position.y - targetCenter.y);
            float distanceToPlayer = Vector3.Distance(transform.position, objectsInArea[i].transform.position);

            // Combine the two distances into a single score using weights
            float score = (distanceY * weightY) + (distanceToPlayer * weightPlayer);

            // Check if this object is valid and has a lower score (closer) than the current closest target
            var playerStateMachine = objectsInArea[i].GetComponent<PlayerStateMachine>();
            if (score < minScore && playerStateMachine != null && !playerStateMachine.IsDead)
            {
                minScore = score;
                closest = objectsInArea[i].transform;
            }
        }

        return closest;
    }

}



