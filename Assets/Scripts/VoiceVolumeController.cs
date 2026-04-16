using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VoiceVolumeController : MonoBehaviour
{
    [Header("把 VoicePanel 底下的 VolumeSlider 拖到這裡")]
    public Slider volumeSlider;  // 改成 Slider

    // 當前場景的 BGMManager 實例
    private BGMManager currentBGM;

    private void Awake()
    {
        // 讓這個物件 (包含這顆 Slider) 在切場景時不被銷毀
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // 切場景前先綁定一次
        BindToCurrentBGMManager();

        // 監聽後續場景載入事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 每次新場景載入後，都去綁定該場景的 BGMManager
        BindToCurrentBGMManager();
    }

    private void BindToCurrentBGMManager()
    {
        // 1. 在當前場景裡找出 BGMManager
        BGMManager found = GameObject.FindObjectOfType<BGMManager>();
        if (found == null)
        {
            Debug.LogWarning("VoiceVolumeController：找不到當前場景的 BGMManager。");
            // 沒有 BGMManager 時把 Slider 關閉互動
            if (volumeSlider != null)
                volumeSlider.interactable = false;
            return;
        }

        // 2. 如果已經綁定同一個就不用再做多餘動作
        if (currentBGM == found) return;

        // 3. 如果之前綁過某個 BGMManager，就先移除舊的監聽
        if (currentBGM != null && volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);

        currentBGM = found;

        // 4. 把滑桿的初始值設為當前 BGMManager.defaultVolume
        if (volumeSlider != null)
        {
            volumeSlider.value = currentBGM.defaultVolume;
            volumeSlider.interactable = true;

            // 5. 綁定新的監聽：滑桿改變時呼叫對應 BGMManager
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float newValue)
    {
        if (currentBGM != null)
            currentBGM.SetVolume(newValue);
    }
}
