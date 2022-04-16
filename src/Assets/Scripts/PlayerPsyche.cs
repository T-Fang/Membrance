using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PsychoBar : MonoBehaviour
{
    public int MaxSanity = 100;
    public int CurrentSanity;
    public PsycheBar psycheBar;
    // Start is called before the first frame update
    
    void Start()
    {
        CurrentSanity = MaxSanity;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1)) {
            mentalDown(25);
        }
        
    }

    void mentalDown(int psycho) {
        CurrentSanity -= psycho;
        psycheBar.setSanity(CurrentSanity);
    }
}
