using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static StatsManager;

public class StatsUI : MonoBehaviour
{
    StatsManager statsManager;
    public Image gutsBar;
    public GameObject skillTreeWindow;
    private void Start()
    {
        statsManager = StatsManager.instance;
    }

    public void OpensSkillTree()
    {
        skillTreeWindow.SetActive(!skillTreeWindow.activeSelf);
    }

    public void UnlockSkillByButton()
    {
        UnlockSkill(EventSystem.current.currentSelectedGameObject.name);
    }

    public void AddGutsExp(int amount)
    {
        statsManager.guts.AddExperience(amount);
        gutsBar.fillAmount = statsManager.guts.GetCurrentExpPercentage();
    }

    public void UnlockSkill(string skill)
    {
        SkillType skillType = skill switch
        {
            "Beggar" => SkillType.Beggar,
            "JunkLooter" => SkillType.JunkLooter,
            "Stealer" => SkillType.Stealer,
            "Crafter" => SkillType.Crafter,
            "Fighter" => SkillType.Fighter,
            _ => SkillType.None
        };

        if (skillType != SkillType.None)
            UnlockSkill(skillType);
    }

    public void UnlockSkill(StatsManager.SkillType skill)
    {
        if (!statsManager.IsSkillUnlocked(skill)) {
            statsManager.UnlockSkill(skill);
        }       
    }
}
