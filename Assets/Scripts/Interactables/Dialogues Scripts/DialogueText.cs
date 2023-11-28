using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Text", menuName = "Dialogues/Create Dialogue Text")]
public class DialogueText : ScriptableObject
{
    public Sprite icon;
    public string speakerName;

    [TextArea(5, 10)]
    public string[] paragraphs;
}
