using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHp;
    public int currentHp;
    public float moveSpeed = 5f;
    public BodyPartDisplay bodyPartDisplay;
    public bool isPlayer = false;
    [SerializeField]
    private float slideDistance = 0.5f;

    public State state;
    private Vector3 slideTargetPosition;
    private Vector3 startingPosition;
    private Action onSlideComplete;

    internal void PlayDamagedAnim()
    {      
        bodyPartDisplay.PlayDamagedAnimation();
    }

    internal void PlayDamagedFallAnim()
    {
        if (currentHp <= 0 && !bodyPartDisplay.IsAnimationOver())
            bodyPartDisplay.PlayDamagedFallAnimation();
    }

    private Action onAttackComplete;
    public enum State
    {
        Idle,
        Sliding,
        Busy
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }


    private void Update()
    {
        switch (state) {
            case State.Idle:
                break;
            case State.Busy:
                if (bodyPartDisplay.IsAnimationOver())
                {
                    onAttackComplete();
                }
                break;
            case State.Sliding:
                float slideSpeed = 10f;
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

                float reachedDistance = 1f;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                {
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;
        }
    }

    public void PlayCombatIdleAnim(bool turnOn)
    {
        bodyPartDisplay.PlayCombatIdleAnimation(turnOn);
        Debug.Log(bodyPartDisplay);
    }
    /*
    slide -> state.sliding
    attack -> state.attacking
    slide -> state.sliding
     */
    public void Attack(CombatUnit targetCombatUnit)
    {
        var slideTargetPosition = targetCombatUnit.GetPosition() + (GetPosition() - targetCombatUnit.GetPosition()).normalized * slideDistance;
        var startingPosition = GetPosition();

        //Slide to target      
        SlideToPosition(slideTargetPosition, () => {
            Damage(targetCombatUnit, () => {
                bodyPartDisplay.FlipBodyPartsByX();
                SlideToPosition(startingPosition, () => { 
                    state = State.Idle; 
                    bodyPartDisplay.FlipBodyPartsByX(); 
                    CombatManager.instance.SetNextState();
                });
            });
        });      
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        if (Math.Ceiling(slideTargetPosition.normalized.x) < Math.Ceiling(this.GetPosition().normalized.x))
            bodyPartDisplay.FlipBodyPartsByX();

        bodyPartDisplay.PlayDashAnimation();

        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
    }

    private void Damage(CombatUnit targetCombatUnit, Action onAttackComplete)
    {
        bodyPartDisplay.PlayAttack1Animation();

        this.onAttackComplete = onAttackComplete;
        state = State.Busy;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
