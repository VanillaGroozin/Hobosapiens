using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public Transform combatArena;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        if (CombatManager.IsCombatActive()) target = combatArena;
        else target = player;
        Follow();
    }
    void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        smoothedPosition.z = 0;
        transform.position = smoothedPosition;
    }
}
