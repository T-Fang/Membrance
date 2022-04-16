using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name.Contains("Player")) {
            // Set player as the child of the moving platform
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.name.Contains("Player")) {
            // Remove the parent from the player
            collision.gameObject.transform.SetParent(null);
        }
    }
}
