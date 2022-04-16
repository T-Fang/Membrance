using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierActivation : MonoBehaviour
{
    // [SerializeField] private GameObject orb;
    [SerializeField] private GameObject[] barriers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // TODO: add orb counter

            // Destroy(gameObject);
            // print(gameObject.name);
            gameObject.SetActive(false);

            foreach (GameObject barrier in barriers)
            {
                barrier.SetActive(false);
            }
            FindObjectOfType<AudioManager>().Play("DeactivateBarrier");
        }
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     if (!orb.activeSelf) {  // orb has been collected/destroyed
    //         foreach (GameObject barrier in barriers) {
    //             barrier.SetActive(false);
    //         }
    //     }
    // }
}
