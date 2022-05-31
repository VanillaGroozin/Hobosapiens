using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    StatsManager statsManager;
    public TextMeshProUGUI LVL;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI STM;
    public TextMeshProUGUI GLD;
    public TextMeshProUGUI EXP;
    public TextMeshProUGUI GTS;
    public TextMeshProUGUI INT;
    public TextMeshProUGUI CHR;
    public TextMeshProUGUI CUN;
    public TextMeshProUGUI VIT;

    void Start()
    {
        statsManager = StatsManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        LVL.text = "LVL:\n" + statsManager.level.GetValue().ToString();
        HP.text = "HP:\n" + statsManager.health.ToString();
        STM.text = "STM:\n" + statsManager.stamina.ToString();
        GLD.text = "GLD:\n" + statsManager.gold.ToString();
        EXP.text = "EXP:\n0";
        GTS.text = "GTS:\n" + statsManager.guts.GetValue().ToString();
        INT.text = "INT:\n" + statsManager.intelligence.GetValue().ToString();
        CHR.text = "CHR:\n" + statsManager.charisma.GetValue().ToString();
        CUN.text = "CUN:\n" + statsManager.cunning.GetValue().ToString();
        VIT.text = "VIT:\n" + statsManager.vitality.GetValue().ToString();
    }
}
