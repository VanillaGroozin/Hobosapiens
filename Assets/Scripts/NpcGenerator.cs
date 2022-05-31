using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGenerator : MonoBehaviour
{
    public GameObject NpcGO;
    public BodySpritePart bodyParts;
    public List<BodySpritePart> bagsParts;
    public List<BodySpritePart> bootsParts;
    public List<BodySpritePart> coatParts;
    public List<BodySpritePart> fhairParts;
    public List<BodySpritePart> hairParts;
    public List<BodySpritePart> glassesParts;
    public List<BodySpritePart> glovesParts;
    public List<BodySpritePart> hatParts;
    public List<BodySpritePart> pantsParts;
    public List<BodySpritePart> shirtParts;

    private void Awake()
    {
        GenerateNpc();
        GenerateNpc();
        GenerateNpc();
        GenerateNpc();
    }

    public void GenerateNpc()
    {
        var position = new Vector3(Random.Range(-3, 3), Random.Range(-13, -3), 1f);

        GameObject newNpc = Instantiate(NpcGO, position, Quaternion.identity);
        newNpc.transform.localScale = new Vector2(1, 1);
        
        var npcBodyPart = newNpc.GetComponentInChildren<BodyPartDisplay>();

        //generate bag
        int rand = Random.Range(0, bagsParts.Count);
        //npcBodyPart.bodyPart = bagsParts[rand];
        npcBodyPart.ChangeBodyPart(bagsParts[rand]);

        //generate boots
        rand = Random.Range(0, bootsParts.Count);
        //npcBodyPart.bodyPart = bootsParts[rand];
        npcBodyPart.ChangeBodyPart(bootsParts[rand]);

        //generate coat
        rand = Random.Range(0, coatParts.Count);
        npcBodyPart.ChangeBodyPart(coatParts[rand]);

        //generate fhair
        rand = Random.Range(0, fhairParts.Count);
        npcBodyPart.ChangeBodyPart(fhairParts[rand]);

        //generate hair
        rand = Random.Range(0, hairParts.Count);
        npcBodyPart.ChangeBodyPart(hairParts[rand]);

        //generate glasses
        rand = Random.Range(0, glassesParts.Count);
        npcBodyPart.ChangeBodyPart(glassesParts[rand]);

        //generate gloves
        rand = Random.Range(0, glovesParts.Count);
        npcBodyPart.ChangeBodyPart(glovesParts[rand]);

        //generate hat
        rand = Random.Range(0, hatParts.Count);
        npcBodyPart.ChangeBodyPart(hatParts[rand]);

        //generate pants
        rand = Random.Range(0, pantsParts.Count);
        npcBodyPart.ChangeBodyPart(pantsParts[rand]);

        //generate shirt
        rand = Random.Range(0, shirtParts.Count);
        npcBodyPart.ChangeBodyPart(shirtParts[rand]);


        //NpcGO.GetComponentInChildren<BodyPartDisplay>() = npcBodyPart;
        npcBodyPart.RefreshBodyParts();

       
        Debug.Log("NPC created");
    }
}
