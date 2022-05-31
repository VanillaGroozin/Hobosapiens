using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerName;
    public Image speakerImage;
    public GameObject dialogBox;
    public GameObject dialogueChooseButton;
    public GameObject currentLine;
    public RectTransform dialoguePanel;
    private Speaker speaker;
    public bool isDialogueActive = false;
    private StatsManager statsManager;

    public TextMeshProUGUI speakerText;

    #region Singleton
    public static DialogueManager instance = null;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple DialogueManagers!");
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        dialogBox.SetActive(false);
        statsManager = StatsManager.instance;
        
    }
    public void StartDialogue(Speaker speaker)
    {
        isDialogueActive = true;
        this.speaker = speaker;
        dialogBox.SetActive(true);
        speakerName.text = speaker.name;
        speakerImage.sprite = speaker.speakerSprite;

        ClearChooseOptions();
        ContinueDialogue(speaker.dialogueLines.Where(x => x.id == speaker.dialogueLines.Min(x => x.id)).FirstOrDefault()); //first phrase should be always a greeting
        LoadGeneralLines();

        LayoutRebuilder.ForceRebuildLayoutImmediate(dialoguePanel); //does it work? (upd: it does)
    }
    private void ContinueDialogue(DialogueLine dialogueLine)
    {
        if (dialogueLine is null)
        {
            EndDialogue();
            return;
        }      

        ClearChooseOptions();
        var dialogueLines = dialogueLine.isGeneral ? speaker.generalDialogueLines : speaker.dialogueLines;
        var previousChoosenLine = dialogueLines.Where(x => x.id == dialogueLine.parentId).FirstOrDefault();

        var alternativeLines = dialogueLines.Where(x => x.parentId == dialogueLine.parentId && ValidRequirements(x)).ToArray();
        if (alternativeLines.Count() > 1)
        {
            int randomedAlternativeLine = UnityEngine.Random.Range(0, alternativeLines.Count());
            dialogueLine = alternativeLines[randomedAlternativeLine];
        }

        if (previousChoosenLine != null)
            SetDialogueOptionEffects(previousChoosenLine);

        speakerText.text = dialogueLine.text;
        LoadPlayerPhrase(dialogueLine);

        LayoutRebuilder.ForceRebuildLayoutImmediate(dialoguePanel);
    }
    public void EndDialogue()
    {
        isDialogueActive = false;
        dialogBox.SetActive(false);
    }


    private void LoadGeneralLines(int id = 0)
    {
        foreach (var dialogueLine in speaker.generalDialogueLines.Where(x => x.parentId == id))
        {
            if (!ValidRequirements(dialogueLine)) continue;

            if (id != 0)
                speakerText.text = dialogueLine.text;
            CreateOptionButton(dialogueLine);
        }
    }

    private void ClearChooseOptions()
    {
        for (int i = 0; i < dialoguePanel.childCount; i++)
        {
            Transform child = dialoguePanel.GetChild(i);
            if (child.tag == "DialogueOption") Destroy(child.gameObject);
        }
    }

    private void SetDialogueOptionEffects(DialogueLine choosenOption)
    {
        foreach (var effect in choosenOption.effects.Split(','))
        {
            if (effect == string.Empty) break;
            var effectName = effect.Split(':')[0];
            var effectValue = effect.Split(':')[1];

            if (effectName == "QUEST")
                FindObjectOfType<QuestManager>().SetQuestById(Convert.ToInt32(effectValue));
            else if (effectName == "QUEST_COMPLETE")
                FindObjectOfType<QuestManager>().QuestCompete(Convert.ToInt32(effectValue));
            else if (effectName == "START")
                FindObjectOfType<CombatManager>().StartCombat();
            else
                FindObjectOfType<StatsManager>().ModifyStat(effectName, effectValue);
        }
    }

    private void LoadPlayerPhrase(DialogueLine currentLine)
    {
        IEnumerable<DialogueLine> dialogueLines = currentLine.isGeneral ? speaker.generalDialogueLines : speaker.dialogueLines;

        if (currentLine.same_childs_as_dialogue_id == string.Empty)
            dialogueLines = dialogueLines.Where(x => x.parentId == currentLine.id);
        else
            dialogueLines = dialogueLines.Where(x => x.parentId.ToString() == currentLine.same_childs_as_dialogue_id);

        foreach (var dialogueLine in dialogueLines)
        {
            if (!ValidRequirements(dialogueLine)) continue;

            CreateOptionButton(dialogueLine);
        }
    }


    private bool ValidRequirements(DialogueLine chooseOption)
    {
        foreach (var requirement in chooseOption.requirements.Split(','))
        {
            if (requirement == string.Empty) break;

            var requirementName = requirement.Split(':')[0].Trim();
            var requirementValue = requirement.Split(':')[1].Trim();

            return requirementName switch
            {
                "LIFEPATH" => statsManager.GetStatValue("LIFEPATH").ToString() == requirementValue,
                "QUEST" => FindObjectOfType<QuestManager>().currentQuests.Exists(x => x.id.ToString() == requirementValue),
                "SKILL" => statsManager.IsSkillUnlocked(requirementValue),
                "CHAR" => statsManager.charisma.GetValue() >= int.Parse(requirementValue),
                "VIT" => statsManager.vitality.GetValue() >= int.Parse(requirementValue),
                "GUTS" => statsManager.guts.GetValue() >= int.Parse(requirementValue),
                "CUN" => statsManager.cunning.GetValue() >= int.Parse(requirementValue),
                "INT" => statsManager.intelligence.GetValue() >= int.Parse(requirementValue),
                _ => false
            };
        }
        return true;
    }
   

    private void CreateOptionButton(DialogueLine optionLine)
    {
        GameObject chooseButton = Instantiate(dialogueChooseButton);
        chooseButton.transform.SetParent(dialoguePanel.transform, false);
        chooseButton.transform.localScale = new Vector2(1, 1);
        chooseButton.GetComponentInChildren<TextMeshProUGUI>().text = optionLine.text;
        Button tempButton = chooseButton.GetComponent<Button>();

        var dialogueLines = optionLine.isGeneral ? speaker.generalDialogueLines : speaker.dialogueLines;
        DialogueLine nextNpcLine;

        if (optionLine.same_childs_as_dialogue_id == string.Empty)
            nextNpcLine = dialogueLines.Where(x => x.parentId == optionLine.id).FirstOrDefault();
        else
            nextNpcLine = dialogueLines.Where(x => x.parentId.ToString() == optionLine.same_childs_as_dialogue_id).FirstOrDefault();

        tempButton.onClick.AddListener(() => ContinueDialogue(nextNpcLine));
    }



    #region old_code
    //private void ButtonClicked(int tempInt)
    //{
    //    StartDialogueByID(tempInt);
    //}

    //private void StartDialogueByID(int currentDialogueId)
    //{
    //    var dialogueLine = speaker.dialogueLines.Where(x => x.id == currentDialogueId).FirstOrDefault(); //Can be only one

    //    ClearChooseOptions();

    //    SetDialogueOptionEffects(dialogueLine);

    //    if (dialogueLine.speakerId == 1) { //if player            
    //        FillDialogueLine(dialogueLine); //for players 
    //        ContinueDialogueByNpc(dialogueLine);
    //    } 
    //    else {
    //        FillDialogueLine(dialogueLine); //for NPC        
    //        FillChooseOptions(GetDialogueChildOptions(dialogueLine)); //for player - childsId
    //    }     

    //    LayoutRebuilder.ForceRebuildLayoutImmediate(dialoguePanel); //does it works?
    //}

    //private void ContinueDialogueByNpc(DialogueLine dialogueLine)
    //{
    //    DialogueLine npcNextDialogueLine = null;

    //    if (dialogueLine.same_childs_as_dialogue_id == string.Empty)
    //        npcNextDialogueLine = speaker.dialogueLines.Where(x => x.parentId == dialogueLine.id).FirstOrDefault();
    //    else
    //        npcNextDialogueLine = speaker.dialogueLines.Where(x => x.parentId.ToString() == dialogueLine.same_childs_as_dialogue_id).FirstOrDefault();

    //    StartDialogueByID(npcNextDialogueLine.id);
    //}

    //private void ClearDialoguePanel()
    //{
    //    while (dialoguePanel.childCount > 1)
    //    {
    //        Transform child = dialoguePanel.GetChild(1);
    //        child.parent = null;
    //        Destroy(child.gameObject);
    //    }
    //}

    // private void ClearChooseOptions()
    // {
    //    for (int i = 0; i < dialoguePanel.childCount; i++)
    //    {
    //        Transform child = dialoguePanel.GetChild(i);
    //        if (child.tag == "DialogueOption") Destroy(child.gameObject);
    //    }
    // }

    //private IEnumerable<DialogueLine> GetDialogueChildOptions(DialogueLine currentNpcPhrase)
    //{
    //    IEnumerable<DialogueLine> childOptions = null;
    //    if (currentNpcPhrase.same_childs_as_dialogue_id != string.Empty) //Look for repeating options
    //        childOptions = speaker.dialogueLines.Where(x => x.parentId.ToString() == currentNpcPhrase.same_childs_as_dialogue_id);
    //    else //Look by parents
    //        childOptions = speaker.dialogueLines.Where(x => x.parentId == currentNpcPhrase.id);
    //    return childOptions;
    //}

    //private void CreateOptionButton(DialogueLine optionLine)
    //{
    //    GameObject chooseButton = Instantiate(dialogueChooseButton);
    //    chooseButton.transform.SetParent(dialoguePanel.transform, false);
    //    //chooseButton.transform.parent = dialoguePanel.transform;
    //    chooseButton.transform.localScale = new Vector2(1, 1);
    //    chooseButton.GetComponentInChildren<TextMeshProUGUI>().text = optionLine.text;
    //    Button tempButton = chooseButton.GetComponent<Button>();


    //    DialogueLine nextNpcLine;

    //    if (optionLine.same_childs_as_dialogue_id == string.Empty)
    //        nextNpcLine = speaker.dialogueLines.Where(x => x.parentId == optionLine.id).FirstOrDefault();
    //    else
    //        nextNpcLine = speaker.dialogueLines.Where(x => x.parentId.ToString() == optionLine.same_childs_as_dialogue_id).FirstOrDefault();


    //    tempButton.onClick.AddListener(() => LoadLine(nextNpcLine.id));
    //}

    //private void FillChooseOptions(IEnumerable<DialogueLine> childOptions)
    //{          
    //    //Get dialogue options from current dialogueId
    //    foreach (var childOption in childOptions)
    //    {
    //        if (!ValidRequirements(childOption)) continue;

    //        CreateOptionButton(childOption);
    //    }
    //}

    //private bool ValidRequirements(DialogueLine chooseOption)
    //{
    //    foreach (var requirement in chooseOption.requirements.Split(','))
    //    {
    //        if (requirement == string.Empty) break;

    //        var requirementName = requirement.Split(':')[0];
    //        var requirementValue = requirement.Split(':')[1];

    //        return requirementName switch
    //        {
    //            "LIFEPATH" => FindObjectOfType<StatsManager>().GetStatValue("LIFEPATH").ToString() == requirementValue,
    //            "QUEST" => FindObjectOfType<QuestManager>().currentQuests.Exists(x => x.id.ToString() == requirementValue),
    //            _ => false
    //        };
    //    }
    //    return true;
    //}

    //private void SetDialogueOptionEffects(DialogueLine choosenOption)
    //{
    //    foreach (var effect in choosenOption.effects.Split(','))
    //    {
    //        if (effect == string.Empty) break;
    //        var effectName = effect.Split(':')[0];
    //        var effectValue = effect.Split(':')[1];

    //        if (effectName == "QUEST")
    //            FindObjectOfType<QuestManager>().SetQuestById(Convert.ToInt32(effectValue));
    //        else if (effectName == "QUEST_COMPLETE")
    //            FindObjectOfType<QuestManager>().QuestCompete(Convert.ToInt32(effectValue));
    //        else
    //            FindObjectOfType<StatsManager>().ModifyStat(effectName, effectValue);
    //    }            
    //}

    //private void FillDialogueLine (DialogueLine dialogueLine)
    //{
    //    AddDialogueLine(dialoguePanel, $"{dialogueLine.speakerName}: -{dialogueLine.text}{"\n"}");
    //}


    //private void AddDialogueLine(RectTransform textPanel, string line)
    //{
    //    GameObject dialogueLineGO = Instantiate(dialogueLine);
    //    dialogueLineGO.transform.SetParent(dialoguePanel.transform, false);
    //    dialogueLineGO.transform.localScale = new Vector2(1, 1);
    //    dialogueLineGO.GetComponentInChildren<TextMeshProUGUI>().text = line;
    //}
    #endregion
}
