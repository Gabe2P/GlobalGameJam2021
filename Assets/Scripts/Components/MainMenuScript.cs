using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public string nextScene = "GGJ";

    public void ChangeScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(nextScene);
        }
    }

    public void Quit()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
    }
}
