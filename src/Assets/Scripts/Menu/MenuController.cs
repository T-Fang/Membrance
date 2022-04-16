using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaultVolume = 10.0f;

    [Header("Levels to Load")]
    public string newGameLevel;
    private string levelToLoad;


    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume == 0 ? 0 : volume / 100.0f;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
        StartCoroutine(Confirmation());
    }

    public void ResetVolume()
    {
        AudioListener.volume = defaultVolume;
        volumeTextValue.text = defaultVolume.ToString("0.0");
        volumeSlider.value = defaultVolume;
        VolumeApply();
    }

    public IEnumerator Confirmation()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
