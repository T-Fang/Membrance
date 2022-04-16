using System;
using UnityEngine;
using System.Collections;

public class Trap_Lava : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.GetComponent<Player>().InIFrame)
        {
            StartCoroutine(WaitABit());
            // Instant death
            collision.GetComponent<Health>().TakeDamage(Int32.MaxValue);
            //Player.SingoTone.MakeInvincible();
            collision.GetComponent<Player>().MakeInvincible();
        }
    }

    private IEnumerator WaitABit()
    {
        //Wait for 4 seconds
        Debug.Log("waiting");
        yield return new WaitForSecondsRealtime(5);
    }
}
