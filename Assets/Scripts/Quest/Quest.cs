using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public bool isActive;

    public int id;
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
    public Quest() { }
    public Quest(int id, string title, string description, int experienceReward, int goldReward)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.experienceReward = experienceReward;
        this.goldReward = goldReward;
    }

    
}
