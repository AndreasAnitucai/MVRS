using System.Collections;
using System.Collections.Generic;
using System.Threading;
using tripolygon.UModeler;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;
    public float speed = 5;
    public float waitTime = 1f;
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject uIScore;

    private void Start()
    {
        uIScore = GameObject.FindGameObjectWithTag("Score");
        player = GameObject.FindGameObjectWithTag("Player");
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
        this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        int targetWaypointIndex = 1;
        int tempWaypointIndex = 0;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        while(true)
        {
            if(this.gameObject.GetComponent<FieldOfView>().canSeePlayer)
            {
                Vector3 playerPos = player.transform.position;
                agent.speed = 6;
                agent.stoppingDistance = 2;
                agent.autoBraking = false;
                agent.SetDestination(playerPos);
                float distance = Vector3.Distance(playerPos, this.gameObject.transform.position);
                if(distance <= 2)
                {
                    //Game over
                    uIScore.transform.GetComponent<Score>().LostGame();
                    //Debug.Log("cake");
                    StartCoroutine(PleaseJustReloadTheMap());
                }
                IEnumerator PleaseJustReloadTheMap()
                {
                    yield return new WaitForSeconds(10f);
                    Application.Quit();
                    //SceneManager.LoadScene("Map01");
                }
                for (int i = 0; i < 5; i++)//prevents inf Loop
                {
                    yield return null;
                }
                    for (int j = 0; j < waypoints.Length; j++)
                    {
                        float distTmp = Vector3.Distance(this.gameObject.transform.position, waypoints[tempWaypointIndex]);
                        float distOri = Vector3.Distance(this.gameObject.transform.position, waypoints[j]);
                        //if (waypoints[j]/*targetWaypointIndex=+1]*/ != null)
                        //{
                            if (distTmp >= distOri)
                            {
                                targetWaypointIndex = j;
                                //Debug.Log("What: " + j);
                                targetWaypoint = waypoints[j];
                                tempWaypointIndex = j;
                            }
                        //}

                    }

            }
            else
            {
                agent.speed = 5;
                agent.stoppingDistance = 0;
                agent.autoBraking = true;
                agent.SetDestination(targetWaypoint);
                //Debug.Log("Name: "+ this.gameObject.name +" cake: " + targetWaypointIndex + " Vector3: "+targetWaypoint + " modul: " + (targetWaypointIndex + 1) % waypoints.Length);
                if (Mathf.Approximately(transform.position.x, targetWaypoint.x) && Mathf.Approximately(transform.position.z, targetWaypoint.z))
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;

                    targetWaypoint = waypoints[targetWaypointIndex];
                    yield return new WaitForSeconds(waitTime);
                }
                yield return null;
            }
            
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
