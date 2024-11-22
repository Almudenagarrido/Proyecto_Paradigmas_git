using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportCollider : MonoBehaviour
{
    public Vector2 newPosition;
    public int numberBlackhole;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Blackhole"))
        {
            transform.position = newPosition;
        }
    }
}
