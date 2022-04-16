using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFollower : MonoBehaviour
{
    [SerializeField] private Transform Target;
    void Update()
    {
        transform.position = new Vector3(Target.position.x, Target.position.y, -1);
    }
}
