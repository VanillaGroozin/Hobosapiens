using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public string _name;
    public Item item;
    public BodySpritePart bodySpritePart;
    public bool canPickUp;
    public EquipType equipType;
    public float buyPrice;
    public float sellPrice;
    public bool isConsumable;
    public int food;
    public enum EquipType
    {
        HAT = 1,
        BAG = 2,
        SHIRT = 3,
        BOOTS = 4,
        GLASSES = 5,
        GLOVES = 6,
        COAT = 7,
        PANTS = 8
    }

    private void Start()
    {
        sellPrice = buyPrice * 0.4f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canPickUp)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        if (Inventory.instance.Add(this))
        {
            //FindObjectOfType<BodyPartDisplay>().ChangeBodyPart(bodySpritePart);
            gameObject.SetActive(false);
        }      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickUp = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickUp = false;
        }
    }

    public bool Use()
    {
        Debug.Log("item used");
        StatsManager.instance.AddFood(this.food);
        
        return true;
    }

}
