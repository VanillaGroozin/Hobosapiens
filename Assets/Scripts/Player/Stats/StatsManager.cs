using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    #region Singleton
    public static StatsManager instance = null;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple StatsManagers!");
            Destroy(gameObject);
        }
    }
    #endregion




    #region Skills
    public GameObject craftTab;

    public enum SkillType
    {
        None,
        Beggar,
        JunkLooter,
        Stealer,
        Crafter,
        Fighter
    }
    private List<SkillType> unlockedSkillTypeList;

    public void UnlockSkill(SkillType skillType)
    {
        if (unlockedSkillTypeList.Contains(skillType)) return;

        unlockedSkillTypeList.Add(skillType);
        CheckSubunlocks(skillType);
        Debug.Log($"{skillType} unlocked!");
    }

    private void CheckSubunlocks(SkillType skillType)
    {
        if (skillType == SkillType.Crafter)
            craftTab.GetComponent<TabBtn>().Enable();     
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        if (!unlockedSkillTypeList.Contains(skillType)) Debug.Log($"Can't use {skillType}!");
        return unlockedSkillTypeList.Contains(skillType);
    } 

    public bool IsSkillUnlocked(string skillStr)
    {
        skillStr = skillStr.ToLower();
        SkillType skill = skillStr switch
        {
            "beggar" => SkillType.Beggar,
            _ => SkillType.None
        };
        return IsSkillUnlocked(skill);       
    }
    #endregion





    public enum LifePath
    {
        TRAVELLER = 1,
        WORKER = 2,
        LOVER = 3,
        LOST = 4,
        MEMORYLOSS = 5,
    }

    public bool CanBuy(Collectable item)
    {
        return gold >= item.buyPrice;
    }

    public void Buy(Collectable item)
    {
        gold -= item.buyPrice;
    }

    public Stat level;
    public int health = 0;
    public int stamina = 0;
    public int food = 0;
    public float gold = 0;


    public Stat lifepath;


    public Stat guts;
    public Stat intelligence;
    public Stat charisma;
    public Stat cunning;
    public Stat vitality;

    private void Start()
    {
        //old names
        lifepath.name = "LIFEPATH";
        guts.name = "GUTS";
        cunning.name = "CUN";
        intelligence.name = "INT";
        charisma.name = "CHAR";
        vitality.name = "VIT";

        unlockedSkillTypeList = new List<SkillType>();

        InvokeRepeating("IncreaseHunger", 0, 2.5f);
    }

    public void IncreaseHunger()
    {
        food--;
        if (food < 0) food = 0;
    }
    
    public void AddFood(int points)
    {
        food += points;
    }

    public int GetStatValue(string statName)
    {
        Stat charachterStatToGet = GetStat(statName);
        return charachterStatToGet.GetValue();
    }

    private Stat GetStat(string statStr)
    {
        return statStr switch
        {
            "GUTS" => this.guts,
            "CUN" => this.cunning,
            "VIT" => this.vitality,
            "CHAR" => this.charisma,
            "INT" => this.intelligence,
            "LIFEPATH" => this.lifepath,
            _ => null
        };
    }
    public void ModifyStat(string statName, string statValue)
    {
        Stat charachterStatToModify = GetStat(statName);

        if (statName == "LIFEPATH")
            charachterStatToModify.ModifyValue(statValue);
        else
            charachterStatToModify.ModifyValueWithOperator(statValue);
    }

    public void AddQuestReward(Quest quest)
    {
        level.AddExperience(quest.experienceReward);
        gold += quest.goldReward;
    }
}

