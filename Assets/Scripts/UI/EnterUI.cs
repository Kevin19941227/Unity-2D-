using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] public GameObject panel;  // 指向你的 Panel

    // 連給 Button OnClick 呼叫
    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }
}