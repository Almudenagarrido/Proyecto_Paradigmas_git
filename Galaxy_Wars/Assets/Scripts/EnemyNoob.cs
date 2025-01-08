using UnityEngine;

public class EnemyNoob : MonoBehaviour
{
    public float minVelocidad = 2f;
    public float maxVelocidad = 5f;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;

        // Calcular dirección inicial
        direccion = ObtenerDireccionMovimiento(puntoDeSalida);
        RotarHaciaDireccion();

        // Calcular velocidad inicial
        velocidad = Random.Range(minVelocidad, maxVelocidad);
    }

    void Update()
    {
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

    Vector2 ObtenerDireccionMovimiento(Vector3 puntoDeSalida)
    {
        Vector2 centroPantalla = Vector2.zero;
        Vector2 direccionCalculada = (centroPantalla - (Vector2)puntoDeSalida).normalized;
        return direccionCalculada;
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }
}
