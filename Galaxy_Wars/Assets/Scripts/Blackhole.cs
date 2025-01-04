using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public int blackholeNumber;
    private float rotationSpeed = 100f;
    private Player player;

    void Start()
    {
        
    }

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
