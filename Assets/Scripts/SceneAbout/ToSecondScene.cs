using BulletHell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSecondScene : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        // °²³]ª±®a¦³ tag "Player"
        if (other.CompareTag("Player"))
        {
            //SceneManager.LoadScene(1);
            TransitionManager.I.GoToScene("SecondScene");
        }
    }
}
