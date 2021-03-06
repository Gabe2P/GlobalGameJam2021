//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, ICallAudioEvents
{
    public event Action<string, float> PlayAudio;
    public event Action<string> StopAudio;

    [SerializeField] private InputController input = null;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private Text scoreTextBox;

    public static GameManager _Instance;

    public static GameManager Instance { get { return _Instance; } }



    [SerializeField]
    private int Score;

    [SerializeField]
    private int LoseState = 3;

    [SerializeField]
    private int MissedDeliveries = 0;



    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        PlayAudio?.Invoke("ThemeSong", 0f);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            PlayAudio?.Invoke("ThemeSong", 0f);
            StopAudio?.Invoke("ExpositionSong");
            StopAudio?.Invoke("GameSong");
        }
        if (SceneManager.GetActiveScene().name.Equals("Exposition"))
        {
            PlayAudio?.Invoke("ExpositionSong", 0f);
            StopAudio?.Invoke("ThemeSong");
            StopAudio?.Invoke("GameSong");
        }
        if (SceneManager.GetActiveScene().name.Equals("GGJ"))
        {
            PlayAudio?.Invoke("GameSong", 0f);
            StopAudio?.Invoke("ExpositionSong");
            StopAudio?.Invoke("ThemeSong");
        }

        if (input == null)
        {
            input = FindObjectOfType<InputController>();
            SubscribeToInput(input);
        }

        if(MissedDeliveries >= LoseState)
        {
            GameOver(Score);
        }

    }


    public void AddScore(int points)
    {
        Score += points;
    }
    public void RemoveScore(int points)
    {
        Score -= points;
    }

    public void MissedDelivery()
    {
        MissedDeliveries += 1;
    }

    public void GameOver(float score)
    {
        MissedDeliveries = 0;
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
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
        if(gameOverScreen.active)
        {
            gameOverScreen.SetActive(false);
        }
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
