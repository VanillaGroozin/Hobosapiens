using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableObject : MonoBehaviour
{
    LootManager lootManager;
    public bool lootGenerated;
    public List<Collectable> collectables = new List<Collectable>();
    public enum LootableType
    {
        TrashCan,
        EpicTrashCan
    }

    private void Start()
    {
        lootManager = LootManager.instance;
    }

    public LootableType lootableType;
    private bool canLoot;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canLoot)
        {
            if (StatsManager.instance.IsSkillUnlocked(StatsManager.SkillType.JunkLooter))
                lootManager.Loot(this, lootableType);
            else Debug.Log("Can't loot! No Skill!");
        }

        //if (!canLoot) lootManager.StopLooting();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canLoot = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canLoot = false;
            lootManager.StopLooting();
        }
    }
}
