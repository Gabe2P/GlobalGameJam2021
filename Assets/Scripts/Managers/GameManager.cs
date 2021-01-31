//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Text scoreTextBox;

    private void Update()
    {
        if (input == null)
        {
            input = FindObjectOfType<InputController>();
            SubscribeToInput(input);
        }
    }

    public void GameOver(float score)
    {
        scoreTextBox.text = score.ToString();
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void ToggleScreen()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            return;
        }
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        if (pauseScreen.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SubscribeToInput(InputController reference)
    {
        if (reference != null)
        {
            reference.OnPauseButtonPress += ToggleScreen;
        }
    }

    private void UnsubscribeToInput(InputController reference)
    {
        if (reference != null)
        {
            reference.OnPauseButtonPress -= ToggleScreen;
        }
    }

    private void OnEnable()
    {
        SubscribeToInput(input);
    }

    private void OnDisable()
    {
        UnsubscribeToInput(input);
    }
}
