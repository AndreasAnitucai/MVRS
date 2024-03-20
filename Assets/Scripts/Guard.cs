using Oculus.Interaction.Input;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using tripolygon.UModeler;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;
    public float speed = 5;
    public float waitTime = 1f;
    public NavMeshAgent agent;

    private void Start()
    {

        // Goes in a path
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);
        }

        StartCoroutine(followPath(waypoints)); 
    }
    IEnumerator followPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        while(true)
        {
            Debug.Log("Waypoint[" + targetWaypointIndex + "]");
            agent.SetDestination(targetWaypoint);
            Debug.Log("Cake");
            if (transform.position.x == targetWaypoint.x && transform.position.z == targetWaypoint.z)
            {
                Debug.Log("Waypoint[" + targetWaypointIndex + " (2)] " + (targetWaypointIndex + 1) % waypoints.Length);
                targetWaypointIndex = (targetWaypointIndex+1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }

    }

    //draws spheres and line between waypoints in Scene view
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild (0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

}
