using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFrameSimulator : MonoBehaviour
{
    public Collider2D[] colliders = new Collider2D[0];
    private Player _playerRef;

    public float flickerDelay = 0.1f;
    [Range(0, 1)] public float flickerAlpha = 0.5f;

    public void Awake()
    {
         
    }
}
