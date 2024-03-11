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
    public float waitTime = .3f;
    public NavMeshAgent agent;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());

        // Goes in a path
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);
        }

        StartCoroutine(followPath(waypoints)); 
    }


    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(.2f);


        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 diractionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, diractionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, diractionToTarget, distanceToTarget, obstructionMask)) canSeePlayer = true;
                else canSeePlayer = false;
            }
            else canSeePlayer = false;
            
        }
        else if (canSeePlayer) canSeePlayer = false;
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
            if (transform.position.x == targetWaypoint.x && transform.position.z == targetWaypoint.z)
            {
                Debug.Log("Waypoint[" + targetWaypointIndex + " (2)]" + (targetWaypointIndex + 1) % waypoints.Length);
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
