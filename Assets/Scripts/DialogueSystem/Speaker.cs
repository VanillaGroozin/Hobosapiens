using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "Speaker")]
public class Speaker : ScriptableObject
{
    public bool canSpeak;
    public int speakerId;
    public string speakerName;
    public Sprite speakerSprite;
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    public List<DialogueLine> generalDialogueLines = new List<DialogueLine>();
    public GameObject npcGO;
}
