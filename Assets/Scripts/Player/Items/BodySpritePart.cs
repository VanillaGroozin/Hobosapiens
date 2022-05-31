using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BodySpriteParts", menuName = "Body/Sprite Parts")]
public class BodySpritePart : ScriptableObject
{
    public AnimationClip idleAnimationClip;
    public AnimationClip lwalkAnimationClip;
    public AnimationClip rwalkAnimationClip;

    public PartType partType;

    public static implicit operator GameObject(BodySpritePart v)
    {
        throw new NotImplementedException();
    }

    public enum PartType
    {
        BODY = 1,
        SHIRT = 2,
        COAT = 3,
        PANTS = 4,
        BOOTS = 5,
        HAIR = 6,
        FACIAL_HAIR = 7,
        HAT = 8,
        GLASSES = 9,
        BAG = 10,
        GLOVES = 11
    }
}
