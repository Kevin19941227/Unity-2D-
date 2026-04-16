using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("UI 元件")]
    public GameObject dialogPanel;
    public Text nameText;
    public Text dialogText;
    public Image avatarImage;

    [Header("對話資料")]
    public DialogueData data;

    private int index = 0;
    private bool isShowing = false;

    void Start()
    {
        if (data != null)
        {
            ShowDialog();
        }
    }

    void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }
    }

    public void ShowDialog()
    {
        if (data == null || data.lines.Length == 0) return;

        dialogPanel.SetActive(true);
        index = 0;
        isShowing = true;
        Time.timeScale = 0f; // 暫停遊戲
        ShowLine(index);
    }

    void ShowLine(int i)
    {
        nameText.text = data.lines[i].speakerName;
        dialogText.text = data.lines[i].sentence;
        avatarImage.sprite = data.lines[i].avatar;
    }

    void NextSentence()
    {
        index++;
        if (index < data.lines.Length)
        {
            ShowLine(index);
        }
        else
        {
            dialogPanel.SetActive(false);
            isShowing = false;
            Time.timeScale = 1f; // 恢復遊戲
        }
    }

    public void SetData(DialogueData newData)
    {
        data = newData;
        ShowDialog();
    }
}
