using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    private GameMaster gm;
    public bool isReached = false;
    [SerializeField] private bool isLast = false;
    [SerializeField] private AudioSource reachCheckPointAudio;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
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
            }
            if (isLast)
            {
                gm.ResetGameState();
                gm.LoadNextLevel();
            }
        }
    }

    public static explicit operator Checkpoint(GameObject v)
    {
        throw new NotImplementedException();
    }
}
