using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string sentence;
    public Sprite avatar;
}
