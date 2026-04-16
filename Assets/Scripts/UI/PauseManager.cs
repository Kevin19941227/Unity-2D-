using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // 暫停時顯示的 UI Panel（可選）
    [SerializeField] private GameObject pauseMenuPanel;

    private bool isPaused = false;

    // 給 UI Button OnClick 呼叫：切換暫停／恢復
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // 暫停遊戲
    private void PauseGame()
    {
        Time.timeScale = 0f;           // 遊戲速度歸零，Update/FixedUpdate 全停
        isPaused = true;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    // 恢復遊戲
    public void ResumeGame()
    {
        Time.timeScale = 1f;           // 恢復正常速度
        isPaused = false;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    // Optional：如果想用鍵盤 Esc 也能退出暫停
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }
}
