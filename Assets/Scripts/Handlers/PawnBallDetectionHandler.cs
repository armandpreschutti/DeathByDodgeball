using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBallDetectionHandler : MonoBehaviour
{
    //public Transform player; // Reference to the player
    public PawnManager _pawnManager;
    public Transform closestBall; // Closest target to the player
    public PlayerStateMachine playerStateMachine;
    private Collider2D[] objectsInArea = new Collider2D[10]; // Fixed size array to store objects in the area
    private int objectCount = 0; // Tracks the number of objects in the array


    private void OnDisable()
    {
        playerStateMachine.ClosestBall = null;
    }
    // Called when an object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && objectCount < objectsInArea.Length)
        {
            objectsInArea[objectCount] = collision;
            objectCount++;
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
        closestBall = GetClosestTarget();
        if (objectCount > 0 && closestBall != null)
        {
            //playerStateMachine.CurrentTarget = mainTarget.gameObject;
            playerStateMachine.ClosestBall = closestBall.gameObject;
            // Here you can set mainTarget as the current target, or perform any logic
        }
        else
        {
            closestBall = null;
            playerStateMachine.CurrentTarget = null;
        }

    }

    // Function to get the closest target to the player
    private Transform GetClosestTarget()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < objectCount; i++)
        {
            float distance = Vector3.Distance(transform.position, objectsInArea[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = objectsInArea[i].transform;
            }
        }

        return closest;
    }
}