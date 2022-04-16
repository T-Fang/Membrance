using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeedback : MonoBehaviour
{
    [SerializeField] private SimpleFlash _simpleFlash;
    [SerializeField] private float _flickerTime;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void PlayFeedback()
    {
        if (_spriteRenderer == null) return;
        _simpleFlash.SetDuraton(_flickerTime);
        _simpleFlash.Flash();
    }
}
