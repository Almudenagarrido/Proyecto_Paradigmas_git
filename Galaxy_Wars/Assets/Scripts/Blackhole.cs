using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public int blackholeNumber;
    private float rotationSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = 0f;
        if (blackholeNumber == 1)
        {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        else if (blackholeNumber == 2)
        {
            rotation = rotationSpeed * Time.deltaTime;
        }

        transform.Rotate(0f, 0f, rotation);
        
    }
}
