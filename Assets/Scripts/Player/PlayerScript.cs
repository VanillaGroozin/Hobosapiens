using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    public float moveSpeed = 5f;
    public BodyPartDisplay bodyPartDisplay;
    private Vector3 lastClickedPos;
    private AgentScript agentScript;
    private bool isMoving;
    private bool wasdMoving;
    private Vector2 movementVector;
    [SerializeField]
    private Transform navMeshDestination;

    #region Singleton
    public static PlayerScript instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogWarning("Multiple PlayerScripts!");
            Destroy(gameObject);
        }
    }
    #endregion


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agentScript = GetComponent<AgentScript>();
    }

  
    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.instance.isDialogueActive && !CombatManager.IsCombatActive())
        { 
            MovePlayerByWASD();
            MovePlayerByNavMesh();
            //MovePlayerByMouse();
            this.GetComponent<NavMeshAgent>().enabled = !wasdMoving;

            AnimateMovement(movementVector);
        }
    }

    private void MovePlayerByNavMesh()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastClickedPos.z = 1;
            isMoving = true;
            navMeshDestination.position = lastClickedPos;
        }
        if (isMoving && transform.position != lastClickedPos && lastClickedPos != new Vector3(0, 0, 0))
        {
            //navMeshDestination.transform.position.z 
            //var a = new Vector3(navMeshDestination.transform.position.x, navMeshDestination.transform.position.y, 1);
            agentScript.SetDestination(navMeshDestination.transform.position);
            movementVector = agentScript.agent.velocity;
            //AnimateMovement(agentScript.agent.velocity);
            //Debug.Log(agentScript.agent.velocity);
        }
        else
        {
            isMoving = false;
            AnimateMovement(new Vector2(0, 0));
        }
    }

    void MovePlayerByWASD()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");

        movementVector = (hAxis != 0 && vAxis != 0) ?
            new Vector2(hAxis * 0.7f, vAxis * 0.7f) : 
            new Vector2(hAxis, vAxis);

        wasdMoving = movementVector != new Vector2(0, 0);
        
        if (wasdMoving)
        {
            navMeshDestination.transform.position = agentScript.transform.position;
            lastClickedPos = new Vector3(0, 0, 0);
        }

        rb.MovePosition(rb.position + movementVector * moveSpeed * Time.fixedDeltaTime);  
    } 

    void MovePlayerByMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastClickedPos.z = 1;
            isMoving = true;
        }
        if (isMoving && transform.position != lastClickedPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, lastClickedPos, moveSpeed * Time.deltaTime);
            movementVector = new Vector2(lastClickedPos.x - transform.position.x, lastClickedPos.y - transform.position.y); 
        }
        else
        {
            isMoving = false;
            AnimateMovement(new Vector2(0, 0));
        }      
    }

    public void AnimateMovement(Vector2 movementVector)
    {
        bodyPartDisplay.UpdateAnimator(movementVector);
    }

    public Vector2 GetMovementVector(Vector2 direction)
    {
        return new Vector2(direction.x - transform.position.x, direction.y - transform.position.y);
    }
}
