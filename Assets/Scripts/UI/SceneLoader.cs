using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 透過場景名稱載入
    public void LoadByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 透過索引載入
    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // 按空白鍵示範切換到 GameScene
}
