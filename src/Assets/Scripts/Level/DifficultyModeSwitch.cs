using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModeSwitch : MonoBehaviour
{
    [SerializeField] public PsycheBar psycheBar;
    [SerializeField] private GameObject[] difficultObjects;
    [SerializeField] private GameObject[] easyObjects;
    private float currentSanity;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject difficultObject in difficultObjects) {
            difficultObject.SetActive(false);
        }

        foreach (GameObject easyObject in easyObjects) {
            easyObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSanity = psycheBar.GetCurrentSanity();

        if (currentSanity <= 25.0) {
            foreach (GameObject difficultObject in difficultObjects) {
                difficultObject.SetActive(true);
            }

            foreach (GameObject easyObject in easyObjects) {
                easyObject.SetActive(false);
            }
        } else {
            foreach (GameObject difficultObject in difficultObjects) {
                difficultObject.SetActive(false);
            }

            foreach (GameObject easyObject in easyObjects) {
                easyObject.SetActive(true);
            }
        }
    }
}
