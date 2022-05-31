using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem
{
    private int level = 0;
    private int experience = 0;
    private int experienceToNextLevel = 100;
    private int skillPoints = 0;

    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceToNextLevel)
        {
            level++;
            skillPoints++;
            experience -= experienceToNextLevel;            
        }
    }
    public float GetCurrentExpPercentage()
    {
        return (float)experience / (float)experienceToNextLevel;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }
}
