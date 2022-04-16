using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameMaster gm;

    public static bool IsGamePaused = false;
    [SerializeField] GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void BackToMainMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gm.ResetGameState();
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        gm.ResetGameState();
        gm.ReloadLevel();
    }
}
