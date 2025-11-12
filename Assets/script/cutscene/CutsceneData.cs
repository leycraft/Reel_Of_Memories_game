using UnityEngine;

[System.Serializable]
public class DialogueLineData
{
    public string speakerName;
    public string dialogueText;
    public string background;
    public string character;
    public string bgm;
    public bool isNarration;
}

[System.Serializable]
public class CutsceneData
{
    public string cutsceneId;
    public string bgm;
    public DialogueLineData[] lines;
}
