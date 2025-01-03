using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public string typeOfButton;
    private GameObject InstrWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Action()
    {
        if (typeOfButton == "Instructions")
        {
            InstrWindow.SetActive(true);
        }
    }
}
