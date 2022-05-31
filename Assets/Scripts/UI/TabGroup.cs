using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabBtn> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabBtn selectedTab;
    public List<GameObject> objectsToSwap;

    public void Awake()
    {
        foreach (var item in objectsToSwap)
        {
            item.SetActive(true);
        }
        foreach (var item in objectsToSwap)
        {
            item.SetActive(false);
        }
    }
    public void Subscribe(TabBtn button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabBtn>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabBtn button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
            button.background.sprite = tabHover;
    }

    public void OnTabExit(TabBtn button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabBtn button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();

        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            objectsToSwap[i].SetActive(i == index);
        }
    }

    public void ResetTabs()
    {
        foreach (var btn in tabButtons)
        {
            if (selectedTab != null && btn == selectedTab) continue;
            btn.background.sprite = tabIdle;
        }
    }
}
