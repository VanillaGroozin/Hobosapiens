using System.Collections.Generic;
using UnityEngine;

public class Conversation
{
    [SerializeField] public List<DialogueLine> allLines = new List<DialogueLine>();
    public DialogueLine GetLineByIndex(int index)
    {
        return allLines[index];
    }

    public int GetLength()
    {
        return allLines.Count - 1;
    }
}
