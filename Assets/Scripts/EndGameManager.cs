using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameManager : MonoBehaviour
{
    public Image endScreenImage;
    public float fadeDuration = 1.5f;       // 淡入秒數
    public float returnDelay = 3f;          // 等待秒數後回主選單

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowVictory()
    {
        Sprite victorySprite = Resources.Load<Sprite>("victory");
        AudioClip victoryAudio = Resources.Load<AudioClip>("victory");

        if (victorySprite != null)
        {
            endScreenImage.sprite = victorySprite;
            if (victoryAudio != null) audioSource.PlayOneShot(victoryAudio);
            StartCoroutine(FadeInAndReturn());
        }
        else
        {
            Debug.LogWarning("找不到 victory.png 或 victory.mp3");
        }
    }

    public void ShowDefeat()
    {
        Sprite defeatSprite = Resources.Load<Sprite>("defeat");
        AudioClip defeatAudio = Resources.Load<AudioClip>("defeat");

        if (defeatSprite != null)
        {
            endScreenImage.sprite = defeatSprite;
            if (defeatAudio != null) audioSource.PlayOneShot(defeatAudio);
            StartCoroutine(FadeInAndReturn());
        }
        else
        {
            Debug.LogWarning("找不到 defeat.png 或 defeat.mp3");
        }
    }

    private IEnumerator FadeInAndReturn()
    {
        endScreenImage.gameObject.SetActive(true);

        Color c = endScreenImage.color;
        c.a = 0;
        endScreenImage.color = c;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            endScreenImage.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene("StartScene");
    }
}
