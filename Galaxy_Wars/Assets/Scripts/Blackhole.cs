using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public int blackholeNumber;
    private float rotationSpeed = 100f;
    private GameObject player;

    public Transform exitWormhole;
    private bool isTeleporting = false;

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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.transform.position = exitWormhole.transform.position;
        }
    }
}
