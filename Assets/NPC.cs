using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private AgentScript agentScript;
    private Vector2 movementVector = new Vector2();
    [SerializeField]
    private Transform navMeshDestination;

    void Start()
    {
        agentScript = GetComponent<AgentScript>();
        //ActAlive();
    }

    private void Update()
    {
        if (movementVector == new Vector2(0, 0))
        {
            MoveToRandomPoint();
        }
        FollowDestination();
    }

    private void FollowDestination()
    {
        agentScript.SetDestination(navMeshDestination.transform.position);
        movementVector = agentScript.agent.velocity;
    }

    public void ActAlive()
    {
        MoveToRandomPoint();
    }

    private void MoveToRandomPoint()
    {
        if (agentScript is null) agentScript = GetComponent<AgentScript>();

        var position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-20, -10), 1f);
        

        navMeshDestination.position = position;
        movementVector = agentScript.agent.velocity;

        //Debug.Log($"NPC MoveToRandomPoint {movementVector}");
    }
}
