using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerDisplay : MonoBehaviour
{
    public Speaker speaker;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && speaker.canSpeak)
        {
            speaker.dialogueLines = DB.GetDialogueLinesBySpeakerId(speaker.speakerId);
            speaker.generalDialogueLines = DB.GetGeneralDialogueLines();
            //Set possible enemy in CM
            CombatManager.instance.enemy = this.gameObject;
            FindObjectOfType<DialogueManager>().StartDialogue(speaker);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            speaker.canSpeak = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            speaker.canSpeak = false;
            FindObjectOfType<DialogueManager>().EndDialogue();
        }
    }
}
