using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilityLoot : MonoBehaviour, SuckableAbility
{
    public float LifeSpan = 10f;
    public float StartTime;

    //public Ability ability;
    public GameObject AbilityGameObject;

    #region Chasing Player

    private Player _playerRef;
    public Transform Target;
    public float MinChaseMult = 2f;
    public float MaxChaseMult = 4f;
    private Vector3 _velocity = Vector3.zero;
    private bool _inProximity = false;
    private bool _shouldChase = false;
    private bool _isChasing = false;
    public GameObject TransitionSmokeParticle;
    private Vector3 SmokeOffset = new Vector3(0.0f,1.0f,0.0f);
    public LayerMask whatIsAbsorbpoint;
    public Transform AbsorbPoint;
    
    public 
    #endregion 
    void Start()
    {
        StartTime = Time.unscaledTime;
        _playerRef = GameObject.Find("Player2").GetComponent<Player>();
        TransitionSmokeParticle.GetComponent<TransitionSmokeController>().Init(this);
    }
    
    // Update is called once per frame
    void Update()
    {
        /*
        if (Time.unscaledTime < LifeSpan + StartTime)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false); // safe method
        }
        */

        if (_shouldChase)
        {
            Debug.Log("Should be chasing: " + _shouldChase);
            ChasePlayer();
        }
    }


    /*
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AbilityVacuum") && _isChasing)
        {
            Debug.Log("Reached here");
                Instantiate(TransitionSmokeParticle, col.gameObject.transform.position + SmokeOffset, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                Destroy(gameObject);
        }
    }
    */

    // OnTriggerEnter2D has a bug where if you stay on top of the object the trigger
    // causes some bs with booleans. Use Stay as workaround for now, might not be the best idea
    // TODO: If you know how to fix please go ahead
    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && _isChasing)
        {
            if (col.gameObject.GetComponent<Player>() == null) return;
            _playerRef = col.gameObject.GetComponent<Player>();
            Instantiate(TransitionSmokeParticle, col.gameObject.transform.position + SmokeOffset, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            // /StartCoroutine(WaitAbsorption(col.gameObject.GetComponent<Player>()));
            // Destroy(gameObject);
            gameObject.SetActive(false); // just hack la just hack
        }
    }


    public bool IsWithinSuccRange()
    {
        return _inProximity;
    }

    public void StartSucking() => _shouldChase = true;
    //public void TriggerAbsorbFinish() => _playerRef.IsAbsorbing = false;
    

    IEnumerator WaitAbsorption(Player player)
    {
        // TODO: Hardcoded wait time, need to be able to signal player not absorbing after smoke animation finish playing
        //_playerRef.IsAbsorbing = false;
        Player.IsAbsorbing = false;
        yield return new WaitForSeconds(1f);
    }
    


    public void ChasePlayer()
    {
        _isChasing = true;
        transform.position = Vector3.Lerp(this.transform.position, Target.position, Time.deltaTime* Random.Range(MinChaseMult,MaxChaseMult));
        transform.localScale = Vector3.Lerp(this.transform.localScale, transform.localScale*0.5f, Time.deltaTime* Random.Range(MinChaseMult,MaxChaseMult));
    }

    public void PrintSuckable()
    {
        Debug.Log("Meow");
    }

    public bool IsAtAbsorbPoint()
    {
        return Physics2D.OverlapCircle(AbsorbPoint.position, 0.18f, whatIsAbsorbpoint);
    }

}