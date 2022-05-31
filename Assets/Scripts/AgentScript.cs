using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour
{
    [SerializeField] Transform target;
    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(target.position);

        ////var movementVector = PlayerScript.instance.GetMovementVector(new Vector2(agent.pathEndPosition.x, agent.pathEndPosition.y));
        ////Debug.Log(agent.velocity);
        ////var movingVector = new Vector2(agent.steeringTarget.x, agent.steeringTarget.y);
        //PlayerScript.instance.AnimateMovement(agent.velocity);
    }
    public void SetDestination(Vector2 position)
    {
        agent.SetDestination(position);
    }

    public Transform GetDestination()
    {
        return target;
    }
}

    
