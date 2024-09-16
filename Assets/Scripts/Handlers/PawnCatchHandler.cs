using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCatchHandler : MonoBehaviour
{
    //public Transform player; // Reference to the player
    Collider2D circleCollider;
    public PawnManager _pawnManager;
    public Transform closestBall; // Closest target to the player
    public PlayerStateMachine playerStateMachine;
    private Collider2D[] objectsInArea = new Collider2D[10]; // Fixed size array to store objects in the area
    private int objectCount = 0; // Tracks the number of objects in the array

    private void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        playerStateMachine.ClosestBall = null;
    }

    // Called when an object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && objectCount < objectsInArea.Length)
        {
            if(collision.GetComponent<BallManager>().owningTeam != _pawnManager.teamId && collision.GetComponent<BallManager>().hasOwner)
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
        closestBall = GetClosestTarget(); // Get the closest target (ball)

        if (objectCount > 0 && closestBall != null)
        {
            // Set the ClosestBall to the closest target
            playerStateMachine.ClosestBall = closestBall.gameObject;
        }
        else
        {
            closestBall = null;
            //playerStateMachine.CurrentTarget = null;
        }
    }

    // Function to get the closest ball to the player's circle collider
    private Transform GetClosestTarget()
    {
        if (circleCollider == null)
        {
            Debug.LogError("No CircleCollider2D attached!");
            return null;
        }

        Vector2 center = circleCollider.bounds.center; // Get center of the collider
        Transform closestTransform = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < objectCount; i++)
        {
            // Get the object's position
            Vector2 objectPosition = objectsInArea[i].transform.position;

            // Find the closest point on the player's circle collider to this object
            Vector2 closestPointToObj = circleCollider.ClosestPoint(objectPosition);

            // Calculate the distance between the center of the collider and the closest point
            float distance = Vector2.Distance(center, closestPointToObj);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTransform = objectsInArea[i].transform; // Set closest object transform
            }
        }

        return closestTransform;
    }

}
