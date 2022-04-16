using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform Target;
    public float MinChaseMult = 3f;
    public float MaxChaseMult = 5f;
    private Vector3 _velocity = Vector3.zero;
    private bool _shouldChase = true;
    //private Animator _anim;
    
    void Start()
    {
        //_velocity = new Vector3(1.0f, 1.0f,1.0f);
        //_anim = GetComponent<Animator>();
    }

    public void StartChasing()
    {
        _shouldChase = true;
    }

    void Update()
    {
        if (_shouldChase)
        {
            //transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * 10f);
            //transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, Time.deltaTime*Random.Range(MinChaseMult,MaxChaseMult));
            transform.position = Vector3.Lerp(this.transform.position, Target.position, Time.deltaTime * Random.Range(MinChaseMult,MaxChaseMult));
        }

    }
}
