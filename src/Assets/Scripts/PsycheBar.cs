using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PsycheBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;

    public void SetMaxSanity(int sanity) {
        slider.maxValue = sanity;
        slider.value = sanity;
    }

    public void setSanity(int sanity) {
        slider.value = sanity;
    }

    public float GetCurrentSanity() {
        return slider.value;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
