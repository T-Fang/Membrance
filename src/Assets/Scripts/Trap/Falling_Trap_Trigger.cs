using System.Collections;
using UnityEngine;


public class FallingTrapTrigger : MonoBehaviour
{
    public bool isFallingTrapTriggered;

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player" && collider.GetComponent<Player>().InIFrame)
        {
            // Activate the falling trap
            isFallingTrapTriggered = true;
        }
    }
}
