using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 選擇性保留跨場景
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckVictory()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("檢查中 → 場上 Enemy 數量：" + enemies.Length);

        foreach (GameObject e in enemies)
        {
            Debug.Log(" - " + e.name + " | ActiveSelf: " + e.activeSelf);
        }

        if (enemies.Length == 1) //BUG 不知道為什麼是1
        {
            FindObjectOfType<EndGameManager>()?.ShowVictory();
        }
    }

}
