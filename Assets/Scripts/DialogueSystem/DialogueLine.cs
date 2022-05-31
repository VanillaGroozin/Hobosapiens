using System.Collections.Generic;
using UnityEngine;


public class DialogueLine
{
    public DialogueLine() { }
    public DialogueLine(int id, int parentId, int speakerId, string same_childs_as_dialogue_id, string text,
        string buttonText, string speakerName, string effects, string requirements, bool isGeneral = false)
    {
        this.id = id;
        this.parentId = parentId;
        this.speakerId = speakerId;
        this.same_childs_as_dialogue_id = same_childs_as_dialogue_id;
        this.text = text;
        this.buttonText = buttonText;
        this.speakerName = speakerName;
        this.effects = effects;
        this.requirements = requirements;
        this.isGeneral = isGeneral;
    }
    public int id { get; set; }
    public int parentId { get; set; }
    public int speakerId { get; set; }
    public string same_childs_as_dialogue_id { get; set; }
    public string text { get; set; }
    public string buttonText { get; set; }
    public string speakerName { get; set; }
    public string effects { get; set; }
    public string requirements { get; set; }
    public bool isGeneral { get; set; }
}
