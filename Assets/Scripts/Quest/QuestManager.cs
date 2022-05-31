using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class QuestManager : MonoBehaviour
{
    public List<Quest> currentQuests = new List<Quest>();
    public GameObject questTrackerObject;
    public RectTransform questTracker;


    #region Singleton
    public static QuestManager instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple QuestManagers!");
            Destroy(gameObject);
        }
    }
    #endregion

    public void SetQuestById(int questId)
    {
        var quest = DB.GetQuestById(questId);
        currentQuests.Add(DB.GetQuestById(questId));

        UpdateQuestTracker();
    }   

    public void QuestCompete(int questId)
    {
        var completedQuest = currentQuests.Find(x => x.id == questId);
        FindObjectOfType<StatsManager>().AddQuestReward(completedQuest);
        currentQuests.Remove(completedQuest);

        UpdateQuestTracker();
    }

    private void UpdateQuestTracker()
    {
        for (int i = 0; i < questTracker.childCount; i++)
        {
            Transform child = questTracker.GetChild(i);
            Destroy(child.gameObject);
        }

        foreach (var quest in currentQuests)
        {
            GameObject questLine = Instantiate(questTrackerObject);
            questLine.transform.SetParent(questTracker.transform, false);
            questLine.transform.localScale = new Vector2(1, 1);
            questLine.GetComponentInChildren<TextMeshProUGUI>().text = quest.title;
        }   
    }
}
