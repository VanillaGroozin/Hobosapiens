using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BodySpritePart;
using static Collectable;



public class BodyPartDisplay : MonoBehaviour
{
    Inventory inventory;
    InventoryUI inventoryUI;
    private Animator _animator;
    public GameObject shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart;
    public bool isPlayer = false;


    private void Update()
    {
        SetRightOrderLayers();
    }

    private void Start()
    {
        inventory = Inventory.instance;
        inventoryUI = InventoryUI.instance;
        inventoryUI.onItemEquipedCallback += UpdateAnimationsFromEquipmentPanel;

        _animator = GetComponent<Animator>();

        SetRightOrderLayers();
    }

    

    public void ChangeSortingLayerTo(string sortingLayer)
    {
        List<GameObject> bodyParts = new List<GameObject>() { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };
        foreach (var part in bodyParts)
        {
            part.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayer;
        }
    }

    public void SetRightOrderLayers()
    {
        List<GameObject> bodyParts = new List<GameObject>() 
        { bodyPart, bootsPart, pantsPart, shirtPart, glovesPart, coatPart, facialHairPart, hairPart, bagPart,  hatPart,  glassesPart};

        for (int i = 1; i < bodyParts.Count; i++)
        {
            bodyParts[i].GetComponent<SpriteRenderer>().sortingOrder = bodyParts[i - 1].GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }


    public void FlipBodyPartsByX()
    {
        List<GameObject> bodyParts = new List<GameObject>() { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };

        foreach (var part in bodyParts)
        {
            part.GetComponent<SpriteRenderer>().flipX = !part.GetComponent<SpriteRenderer>().flipX;
        }
    }
   

    public void UpdateAnimator(Vector2 movement_vector)
    {
        List<GameObject> bodyParts = new List<GameObject>() { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };

        foreach (var part in bodyParts)
        {
            if (!part.activeSelf) continue;

            var bodyPartAnimator = part.GetComponent<Animator>();
            //Debug.Log(movement_vector);
            bodyPartAnimator.SetFloat("Vertical", movement_vector.y);
            bodyPartAnimator.SetFloat("Horizontal", movement_vector.x);
            bodyPartAnimator.SetFloat("Speed", movement_vector.sqrMagnitude);
        }
    }

    public bool IsAnimationOver()
    {
        return bodyPart.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    public void PlayCombatIdleAnimation(bool turnOn)
    {
        PlayAnimationByBool("IsInCombat", turnOn);
    }

    public void PlayDamagedFallAnimation()
    {
        PlayAnimationByTrigger("Dead");
    }

    public void PlayDashAnimation()
    {
        PlayAnimationByTrigger("Dash");
    }

    public void PlayAttack1Animation()
    {
        PlayAnimationByTrigger("Attack1");
    }
    internal void PlayDamagedAnimation()
    {
        PlayAnimationByTrigger("Damaged");
    }

    private void PlayAnimationByBool(string name, bool state)
    {
        List<GameObject> bodyParts = new List<GameObject>() { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };

        foreach (var part in bodyParts)
        {
            if (!part.activeSelf) continue;

            var bodyPartAnimator = part.GetComponent<Animator>();
            bodyPartAnimator.SetBool(name, state);
        }
    }

    private void PlayAnimationByTrigger(string animTrigger)
    {
        List<GameObject> bodyParts = new List<GameObject>() 
        { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };

        foreach (var part in bodyParts)
        {
            if (!part.activeSelf) continue;
            
            var bodyPartAnimator = part.GetComponent<Animator>();
            bodyPartAnimator.SetTrigger(animTrigger);
        }
    }


    public void ChangeBodyPart(BodySpritePart newPart)
    {
        GameObject partToChange = newPart.partType switch
        {
            BodySpritePart.PartType.BODY => bodyPart,
            BodySpritePart.PartType.SHIRT => shirtPart,
            BodySpritePart.PartType.COAT => coatPart,
            BodySpritePart.PartType.PANTS => pantsPart,
            BodySpritePart.PartType.BOOTS => bootsPart,
            BodySpritePart.PartType.HAIR => hairPart,
            BodySpritePart.PartType.FACIAL_HAIR => facialHairPart,
            BodySpritePart.PartType.GLASSES => glassesPart,
            BodySpritePart.PartType.BAG => bagPart,
            BodySpritePart.PartType.GLOVES => glovesPart,
            BodySpritePart.PartType.HAT => hatPart,
            _ => null
        };

        partToChange.SetActive(true);
        partToChange.GetComponent<BodyPart>().ChangeBodyPart(newPart);
        OverrideBodyPartAnimation(partToChange, newPart);
    }

    
    public void ClearBodyPartByEquipPart(EquipType part)
    {
        GameObject partToClear = part switch
        {
            EquipType.HAT => hatPart,
            EquipType.BAG => bagPart,
            EquipType.SHIRT => shirtPart,
            EquipType.BOOTS => bootsPart,
            EquipType.GLASSES => glassesPart,
            EquipType.GLOVES => glovesPart,
            EquipType.COAT => coatPart,
            EquipType.PANTS => pantsPart,
            _ => null
        };
        //OverrideBodyPartAnimation(partToClear, null);
        partToClear.SetActive(false);
        partToClear.GetComponent<BodyPart>().ChangeBodyPart(null);
    }

    public void OverrideBodyPartAnimation(GameObject bodyPart, BodySpritePart newPart)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(bodyPart.GetComponent<Animator>().runtimeAnimatorController);


        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        foreach (var a in aoc.animationClips)
        {
            if (bodyPart.GetComponent<BodyPart>().bodySpritePart != null)
            {
                if (a.name.Contains("idle"))
                {
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, newPart.idleAnimationClip));
                }
                else if (a.name.Contains("lwalk"))
                {
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, newPart.lwalkAnimationClip));
                }
                else if (a.name.Contains("rwalk"))
                {
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, newPart.rwalkAnimationClip));
                }
            }                 
        }

        aoc.ApplyOverrides(anims);

        bodyPart.GetComponent<Animator>().runtimeAnimatorController = aoc;
    }

    public void UpdateAnimationsFromEquipmentPanel()
    {
        if (!isPlayer) return;
        foreach (var slot in inventoryUI.equipedSlots)
        {
            //can be optimized
            if (!object.Equals(slot.item, null))
                ChangeBodyPart(slot.item.bodySpritePart);
            else           
                ClearBodyPartByEquipPart(slot.equipType);
        }
        RefreshBodyParts();
    }

    public void RefreshBodyParts()
    {
        BodyPartsSwapActivity();
        BodyPartsSwapActivity();
    }

    public void BodyPartsSwapActivity()
    {
        List<GameObject> bodyParts = new List<GameObject>() { shirtPart, coatPart, hairPart, facialHairPart, pantsPart, bootsPart, bodyPart, hatPart, glovesPart, glassesPart, bagPart };
        foreach (var bodyPart in bodyParts)
            bodyPart.SetActive(!bodyPart.activeSelf);
    }
}
