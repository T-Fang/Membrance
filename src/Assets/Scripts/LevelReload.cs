using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReload : MonoBehaviour
{

    
    public Animator transition;

    public float transitionTime = 1f;
    public PlayerMovementTest Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.CurrentSanity <= Player.MaxSanity / 2)
        {
            ReloadLevelVariant();
        }
    }

    public void ReloadLevelVariant() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int LevelIndex) {
        transition.SetTrigger("PsychoBreak");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(LevelIndex);
    }
}
