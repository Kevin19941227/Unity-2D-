using UnityEngine;

public class ShowVoicePanel : MonoBehaviour
{
    [Header("把 EscManager/Setting/Voice 這個 Panel 拖到這裡")]
    public GameObject voicePanel;

    /// <summary>
    /// 這個方法會被 Button 的 OnClick 呼叫，用來反轉 Voice Panel 的顯示狀態
    /// </summary>
    public void ToggleVoicePanel()
    {
        if (voicePanel == null)
        {
            Debug.LogWarning("ShowVoicePanel：voicePanel 沒有指派！");
            return;
        }

        bool now = voicePanel.activeSelf;
        voicePanel.SetActive(!now);
    }

    /// <summary>
    /// 如果你想「只開啟」而不反轉，就呼叫這個
    /// </summary>
    public void OpenVoicePanel()
    {
        if (voicePanel != null)
            voicePanel.SetActive(true);
    }

    /// <summary>
    /// 如果你想「只關閉」Call 這個
    /// </summary>
    public void CloseVoicePanel()
    {
        if (voicePanel != null)
            voicePanel.SetActive(false);
    }
}
