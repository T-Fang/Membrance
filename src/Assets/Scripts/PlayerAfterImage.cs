using System;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    // Track time alive
    private float _timeSpawned = 0.0f;
    private float _lifeSpan = 2.0f;

    // alpha values of the afterimage
    private float _alpha = 0.0f;
    private float _alphaDefault = 0.69f;
    private float _alphaFactor = 0.97f; // How fast the image fades

    // Reference to player's transform
    private Transform _playerRef;

    // Reference to afterimage's renderer
    // Reference to player's renderer
    private SpriteRenderer _afterImageSR;
    private SpriteRenderer _playerSR;

    // Color of the afterimage
    private Color _color;
    

    /* OnEnable() implementation.
     * 
     * Get the reference to the sprite renderer of the game object (afterimage)
     * Get the reference to the player (specifically transform component)
     * Get the reference to the sprite renderer of the player 
     *
     * Set the alpha value
     * Assign the player sprite to the appropriate member
     * set the Position of afterimage to the player's position, do the same for rotation
     * Lastly, track the time
     */
    
    private void OnEnable()
    {
        var thisTransform = transform;
        _afterImageSR = GetComponent<SpriteRenderer>();
        _playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSR = _playerRef.GetComponent<SpriteRenderer>();
        _alpha = _alphaDefault;
        
        _afterImageSR.sprite = _playerSR.sprite;
        thisTransform.position = _playerRef.position;
        thisTransform.rotation = _playerRef.rotation;
        _timeSpawned = Time.time;
    }

    /* Update() implementation
     * Update the alpha value
     * Update the color
     * Push the afterimage back to the pool when the time alive is exceeded
     */
    private void Update()
    {
        _alpha *= _alphaFactor;
        //Color PlayerColor = _playerRef.GetComponent<DashColor>().dashColor;
        //_color = new Color(PlayerColor.r, PlayerColor.g, PlayerColor.b, _alpha);
        _color = new Color(0.0f, 1.0f, 0.0f, _alpha);
        //_afterImageSR.material.SetColor("_Color", _color);
        _afterImageSR.color = _color;

        if ((_timeSpawned + _lifeSpan) <= Time.time)
        {
            AfterImagePool.SingleTonInstance.return_to_pool(gameObject); // return the afterimage to pool instead of destructing
        }
    }
}