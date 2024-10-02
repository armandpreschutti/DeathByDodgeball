using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUDodgeDetection : MonoBehaviour
{
    //public Transform player; // Reference to the player
    Collider2D circleCollider;
    public PawnManager _pawnManager;
    public Transform closestBall; // Closest target to the player
    public CPUBrain cpuBrain;
    private Collider2D[] objectsInArea = new Collider2D[10]; // Fixed size array to store objects in the area
    private int objectCount = 0; // Tracks the number of objects in the array


    private void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
        cpuBrain = transform.parent.GetComponentInParent<CPUBrain>();
        _pawnManager = transform.parent.GetComponentInParent<PawnManager>();
    }

    private void OnDisable()
    {
        cpuBrain.closestFreeBall = null;
    }

    // Called when an object enters the trigger area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && objectCount < objectsInArea.Length)
        {
            if (collision.GetComponent<BallManager>().owningTeam != _pawnManager.teamId && collision.GetComponent<BallManager>().hasOwner && collision.GetComponent<BallManager>().isBallActive)
            {
                objectsInArea[objectCount] = collision;
                objectCount++;
            }
        }
    }

    // Called when an object exits the trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveObjectFromArea(collision);
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for balls that have been owned and remove them
        for (int i = 0; i < objectCount; i++)
        {
            if (objectsInArea[i].gameObject == _pawnManager.playerStateMachine.EquippedBall/*GetComponent<BallManager>().hasOwner*/)
            {
                RemoveObjectFromArea(objectsInArea[i]);
                i--; // Adjust the index since the array is shifted after removal
            }
        }

        closestBall = GetClosestTarget(); // Get the closest target (ball)

        if (objectCount > 0 && closestBall != null)
        {
            cpuBrain.closestDodgableBall = closestBall.gameObject; // Set the ClosestBall
        }
        else
        {
            closestBall = null;
            cpuBrain.closestDodgableBall = null;
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

        Vector2 center = circleCollider.bounds.center;
        Transform closestTransform = null;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < objectCount; i++)
        {
            Vector2 objectPosition = objectsInArea[i].transform.position;
            Vector2 closestPointToObj = circleCollider.ClosestPoint(objectPosition);
            float distance = Vector2.Distance(center, closestPointToObj);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTransform = objectsInArea[i].transform;
            }
        }

        return closestTransform;
    }

    // Helper function to remove an object from the objectsInArea array
    private void RemoveObjectFromArea(Collider2D collision)
    {
        for (int i = 0; i < objectCount; i++)
        {
            if (objectsInArea[i] == collision)
            {
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
}
