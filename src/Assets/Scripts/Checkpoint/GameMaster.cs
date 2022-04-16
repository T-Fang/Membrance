using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

// A Singleton class that controls game loading
public class GameMaster : MonoBehaviour
{

    // private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    public Animator transition;
    public float transitionTime = 1f;

    void Awake()
    {
        DontDestroyOnLoad(this);
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    public void ResetGameState()
    {
        lastCheckPointPos = Vector2.zero;
    }

    public void ReloadLevel()
    {
        // TODO: Play death animation before loading scene
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        Debug.Log("Loading level " + levelIndex);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
