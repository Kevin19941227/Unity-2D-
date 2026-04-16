using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager I { get; private set; }
    [SerializeField] private Animator anim;
    [SerializeField] private float waitAfterLoad = 0.1f;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    /// <summary>
    /// 呼叫這個方法來做轉場。
    /// </summary>
    public void GoToScene(string sceneName)
    {
        StartCoroutine(DoTransition(sceneName));
    }

    private IEnumerator DoTransition(string sceneName)
    {
        // 1. 播放淡入（畫面覆蓋）
        anim.SetTrigger("doFadeOut");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // 2. 非同步載入新場景
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f) yield return null;
        op.allowSceneActivation = true;
        yield return new WaitForSeconds(waitAfterLoad);

        // 3. 播放淡出（揭開畫面）
        anim.SetTrigger("doFadeIn");
    }
}
