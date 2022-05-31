using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = Random.Range(0.7f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(animator.speed);
    }
}
