using UnityEngine;

public class EscManager : MonoBehaviour
{
    public static EscManager Instance { get; private set; }

    [Header("設定面板（預設 Inactive）")]
    [Tooltip("把 SettingsPanel（最外層 Panel）拖到這裡")]
    public GameObject settingsPanel;

    private bool isSettingsOpen = false;

    private void Awake()
    {
        // 單例保證
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        else
            Debug.LogWarning("EscManager: settingsPanel 尚未指派！");
    }

    private void Update()
    {
        // 持續監聽 Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    /// <summary>
    /// 外部也可呼叫，用來在按鈕點擊時關閉設定
    /// </summary>
    public void CloseSettings()
    {
        if (isSettingsOpen)
            ToggleSettings();
    }

    /// <summary>
    /// 反轉開關設定面板，大會開啟 → 暫停遊戲，關閉 → 恢復
    /// </summary>
    public void ToggleSettings()
    {
        if (settingsPanel == null) return;

        isSettingsOpen = !isSettingsOpen;
        settingsPanel.SetActive(isSettingsOpen);

        // 同步暫停遊戲與恢復
        Time.timeScale = isSettingsOpen ? 0f : 1f;
    }
}
