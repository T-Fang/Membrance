using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillPickup : MonoBehaviour
{
    private GameMaster gm;
    public bool isReached = false;
    [SerializeField] private bool isLast = false;
    [SerializeField] private AudioSource reachCheckPointAudio;

    string[] skillMessages = {
        "Looks like the orb gave you a new ability along with your memories as well. To Dash, press Z while moving. \n\n" +
            "The orbs appears to suck you in as you find yourself travelling through a portal.",
        "The most recent memory was your greatest yet, so now your newest power is too.  Press T to slow down time for everything except for you \n\n" +
            "This feels familiar. As power wells within you, you are transported to yet another battlefield.",
        "As you collect the final orb, the fog in your mind finally clears and you think to yourself, \"I'm free... finally\"",
        "You can't acutally beat the boss cuz it's bugged, congrats for finishing!"
    };

    [SerializeField] private GameObject dialogue;
    [SerializeField] private TMP_Text dialogueValue;
    [SerializeField] private TMP_Text button;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    void Update()
    {
        if(dialogue.activeSelf && (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.KeypadEnter))) closeDialogue();
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

                if (name.Contains("SkillPickupDash"))
                {
                    setTextAndShowDialogue(skillMessages[0]);
                }
                else if (name.Contains("SkillPickupSlowmo"))
                {
                    setTextAndShowDialogue(skillMessages[1]);
                }
                else if (name.Contains("CheckpointFinal"))
                {
                    setTextAndShowDialogueComplete(skillMessages[3]);
                }
                else
                {
                    setTextAndShowDialogueComplete(skillMessages[2]);
                }
            }
        }
    }

    private void setTextAndShowDialogue(String value)
    {
        Time.timeScale = 0.0f;
        dialogueValue.text = value;
        button.text = "Go to next level";
        dialogue.SetActive(true);
    }

    private void setTextAndShowDialogueComplete(String value)
    {
        Time.timeScale = 0.0f;
        dialogueValue.text = value;
        button.text = "Finish";
        dialogue.SetActive(true);
    }

    public void closeDialogue()
    {
        Time.timeScale = 1.0f;
        dialogue.SetActive(false);
        gm.ResetGameState();
        gm.LoadNextLevel();
    }

    public static explicit operator SkillPickup(GameObject v)
    {
        throw new NotImplementedException();
    }
}
