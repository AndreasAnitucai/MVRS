using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NpcMove : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null )
        {
            Debug.Log("the nav mesh agent component is not attached to "+ gameObject.name);
        }
        else
        {
    
        }
    }

    // Update is called once per frame
    private void SetDestination()
        {
            if(_destination != null)
            {
                Vector3 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
            }
        }
    private void Update()
    {
        SetDestination();
    }
}
