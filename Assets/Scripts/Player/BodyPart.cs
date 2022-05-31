using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public BodySpritePart bodySpritePart;

    public void ChangeBodyPart(BodySpritePart newPart)
    {
        this.bodySpritePart = newPart;
    }

    public void HasDamaged()
    {
        CombatManager.instance.RegisterHit();
    }

    public void HasDamagedToDeath()
    {
        CombatManager.instance.CombatUnitIsDead();
    }
}
