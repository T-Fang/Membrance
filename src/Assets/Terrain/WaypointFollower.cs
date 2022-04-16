using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 0.0001f;

    // Update is called once per frame
    void Update()
    {
        float distanceTolerance = 0.1f;    // instead of 0 to allow for some gap
        float distanceBetweenPlatformAndWaypoint = Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position);

        // If the platform is almost touching the current waypoint
        if (distanceBetweenPlatformAndWaypoint < distanceTolerance) {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) { // reached last waypoint
                currentWaypointIndex = 0;   // cycle back to the first waypoint
            }
        }

        // Move platform a little bit frame by frame
        transform.position = Vector2.MoveTowards(transform.position, 
                            waypoints[currentWaypointIndex].transform.position, 
                            Time.deltaTime * speed);    // Move 2 game units independent of frame rate
    }
}
