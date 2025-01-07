using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WormholeController : MonoBehaviour
{
    public Transform exitWormhole;
    public float tiempoEspera = 2f;  // Tiempo de espera entre teletransportes
    private bool puedeTeletransportar = true;

    private float rotationSpeed = 100f;
    public int wormholeNumber;


    void Update()
    {
        float rotation = 0f;
        if (wormholeNumber == 1)
        {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        else if (wormholeNumber == 2)
        {
            rotation = rotationSpeed * Time.deltaTime;
        }

        transform.Rotate(0f, 0f, rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("BulletPlayer") && puedeTeletransportar)
        {
            exitWormhole.GetComponent<Collider2D>().enabled = false;
            collision.transform.position = exitWormhole.position;
            StartCoroutine(EsperaTeletransporte());
        }
    }
    private IEnumerator EsperaTeletransporte()
    {
        puedeTeletransportar = false;  // Bloquea el teletransporte
        yield return new WaitForSeconds(tiempoEspera);  // Espera el tiempo definido
        exitWormhole.GetComponent<Collider2D>().enabled = true;
        puedeTeletransportar = true;  // Habilita el teletransporte de nuevo
    }
}
