using UnityEngine;

public class EnemyNoob : MonoBehaviour
{
    public float minVelocidad = 3f;
    public float maxVelocidad = 4f;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;
    private Transform jugadorObjetivo;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;

        velocidad = Random.Range(minVelocidad, maxVelocidad);

        // Seleccionar un jugador aleatoriamente
        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");
        if (jugadores.Length > 0)
        {
            jugadorObjetivo = jugadores[Random.Range(0, jugadores.Length)].transform;
        }
        else
        {
            Debug.LogWarning("No se encontraron jugadores con la etiqueta 'Player'.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (jugadorObjetivo != null)
        {
            direccion = (jugadorObjetivo.position - transform.position).normalized;
            RotarHaciaDireccion();
        }

        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    void RotarHaciaDireccion()
    {
        transform.up = direccion;
    }

    Vector3 ObtenerPosicionBorde(int borde)
    {
        float bordeExtra = 1f;
        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        switch (borde)
        {
            case 0: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + bordeExtra, 0); // Arriba
            case 1: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - bordeExtra, 0); // Abajo
            case 2: return new Vector3(-anchoPantalla - bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0); // Izquierda
            case 3: return new Vector3(anchoPantalla + bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0); // Derecha
            default: return Vector3.zero;
        }
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }
}
