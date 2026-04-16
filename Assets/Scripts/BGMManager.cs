using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;
    public float fadeDuration = 1.5f;
    [Range(0f, 1f)] public float defaultVolume = 0.2f; // 加入這行：預設音量 40%

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = defaultVolume; // 設定預設音量

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlaySceneBGM(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneBGM(scene.name);
    }

    public void PlaySceneBGM(string sceneName)
    {
        StartCoroutine(TransitionBGM(sceneName));
    }

    private IEnumerator TransitionBGM(string sceneName)
    {
        yield return StartCoroutine(FadeOut());

        AudioClip newClip = Resources.Load<AudioClip>("Audio/" + sceneName);
        if (newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
            yield return StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("找不到 BGM：" + sceneName);
        }
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private IEnumerator FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.Play();
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, defaultVolume, t / fadeDuration); // 這裡改成 defaultVolume
            yield return null;
        }
        audioSource.volume = defaultVolume;
    }


    //  可選：外部呼叫變更音量
    public void SetVolume(float volume)
    {
        defaultVolume = Mathf.Clamp01(volume);
        audioSource.volume = defaultVolume;
    }
}
