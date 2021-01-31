//Written By Gabriel Tupy 1-30-2021

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField] private GameObject pauseScreen;

    private void Update()
    {
        if (input == null)
        {
            input = FindObjectOfType<InputController>();
            SubscribeToInput(input);
        }
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
