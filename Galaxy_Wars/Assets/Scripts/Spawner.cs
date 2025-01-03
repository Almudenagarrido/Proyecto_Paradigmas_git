using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject meteoritoPrefab;
    public float intervaloGeneracion = 1.5f;


    void Start()
    {
        InvokeRepeating("GenerarMeteorito", 0f, intervaloGeneracion);  // Genera meteoritos indefinidamente
    }

    void GenerarMeteorito()
    {
        Vector3 posicion = ObtenerPosicionBordePantalla();
        Instantiate(meteoritoPrefab, posicion, Quaternion.identity);
    }

    // Genera una posición aleatoria en un borde de la pantalla
    Vector3 ObtenerPosicionBordePantalla()
    {
        Camera camara = Camera.main;

        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        int borde = Random.Range(0, 4);  // Elegir un borde aleatorio

        switch (borde)
        {
            case 0: // Arriba
                return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + 2f, 0);
            case 1: // Abajo
                return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - 2f, 0);
            case 2: // Izquierda
                return new Vector3(-anchoPantalla - 2f, Random.Range(-altoPantalla, altoPantalla), 0);
            case 3: // Derecha
                return new Vector3(anchoPantalla + 2f, Random.Range(-altoPantalla, altoPantalla), 0);
            default:
                return Vector3.zero; // Por si acaso
        }
    }
}
