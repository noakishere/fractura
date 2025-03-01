using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovementBehaviour : MonoBehaviour
{
    // A list of waypoints that can be assigned in the Inspector.
    [SerializeField] private Transform[] waypoints;
    // Speed at which the rat moves.
    [SerializeField] private float moveSpeed = 2f;
    // How long the rat waits at each waypoint.
    [SerializeField] private float waitTime = 2f;

    private bool shouldWalk;

    private void Start()
    {
        shouldWalk = true;
        if (waypoints.Length > 0)
        {
            // Start the movement coroutine.
            StartCoroutine(MoveBetweenWaypoints());
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to RatMovementBehaviour.");
        }
    }

    private IEnumerator MoveBetweenWaypoints()
    {
        while (shouldWalk)
        {
            Transform targetWaypoint = waypoints[Random.Range(0, waypoints.Length)];

            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }
}
