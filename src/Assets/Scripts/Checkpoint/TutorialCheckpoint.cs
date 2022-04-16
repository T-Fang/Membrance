using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCheckpoint : MonoBehaviour
{
    private GameMaster gm;
    public bool isReached = false;
    [SerializeField] private bool isLast = false;
    [SerializeField] private AudioSource reachCheckPointAudio;

    string[] tutorialMessages = {
        "There are numerous traps around so be careful.  Some SPIKE or BURN your health down, while others instantly DROWN you in death (Enter/Esc to exit dialogue)",
        "Hmmm doesn’t seem like you can jump high enough here, the platform seems like it can take you up, try getting on! Be careful not to drop into the water!",
        "It's an enemy patrol! You need to defeat him. Press C or left click to do a melee attack.",
        "It seems this area is blocked off by a barrier. You will need to find the corresponding orb to unlock the barrier!",
        "There are more enemies ahead! These enemies are much stronger, try out your most powerful attack by pressing R or right click. This attack will consume your sanity indicated by the yellow bar on the top left!",
        "The blue orb will grant power like you've never felt before! To use it, stand near the blue orb and press F to absorb the orb.",
    };

    [SerializeField] private GameObject dialogue;
    [SerializeField] private TMP_Text dialogueValue;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            closeTutorialDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            if (!isReached)
            {
                reachCheckPointAudio.Play();
                isReached = true;
                gm.lastCheckPointPos = transform.position;
                Debug.Log("Checkpoint reached, last check point pos updated to: " + gm.lastCheckPointPos);
                
                if (name.Equals("TutorialCheckpoint1"))
                {
                    setTextAndShowDialogue(tutorialMessages[0]);
                }
                else if (name.Equals("TutorialCheckpoint2"))
                {
                    setTextAndShowDialogue(tutorialMessages[1]);
                }
                else if (name.Equals("TutorialCheckpoint3"))
                {
                    setTextAndShowDialogue(tutorialMessages[2]);
                }
                else if (name.Equals("TutorialCheckpoint4"))
                {
                    setTextAndShowDialogue(tutorialMessages[3]);
                }
                else if (name.Equals("TutorialCheckpoint5"))
                {
                    setTextAndShowDialogue(tutorialMessages[4]);
                }
                else if (name.Equals("TutorialCheckpoint6"))
                {
                    setTextAndShowDialogue(tutorialMessages[5]);
                }
            }
        }
    }

    private void setTextAndShowDialogue(String value)
    {
        dialogueValue.text = value;
        dialogue.SetActive(true);
    }

    public void closeTutorialDialogue()
    {
        dialogue.SetActive(false);
    }

    public static explicit operator TutorialCheckpoint(GameObject v)
    {
        throw new NotImplementedException();
    }
}
