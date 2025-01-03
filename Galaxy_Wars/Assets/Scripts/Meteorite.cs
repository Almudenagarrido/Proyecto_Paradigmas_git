using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public float minVelocidad;  // min velocidad del meteorito
    public float maxVelocidad;  // max velocidad del meteorito
    private Vector2 direccion;    // Direcci�n en la que se mueve el meteorito

    private Camera camara;        // C�mara principal

    private float velocidad;

    void Start()
    {
        camara = Camera.main;  // Obtener la c�mara principal

        // Determinar desde qu� borde aparece el meteorito
        int borde = Random.Range(0, 4);  // Elige un borde aleatorio (arriba, abajo, izquierda, derecha)
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);

        // Establecer la posici�n inicial
        transform.position = puntoDeSalida;

        // Calcular la direcci�n del meteorito
        direccion = ObtenerDireccionMovimiento(borde);
    }

    void Update()
    {
        velocidad = Random.Range(minVelocidad, maxVelocidad);
        // Mover el meteorito en la direcci�n calculada
        transform.Translate(direccion * velocidad * Time.deltaTime);

        // Si el meteorito sale de la pantalla, destruirlo
        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    // Funci�n para determinar desde qu� borde sale el meteorito
    Vector3 ObtenerPosicionBorde(int borde)
    {
        float bordeExtra = 1f; // Margen adicional fuera de la pantalla

        // Obtener las dimensiones de la c�mara
        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        switch (borde)
        {
            case 0: // Arriba
                return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + bordeExtra, 0);
            case 1: // Abajo
                return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - bordeExtra, 0);
            case 2: // Izquierda
                return new Vector3(-anchoPantalla - bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            case 3: // Derecha
                return new Vector3(anchoPantalla + bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            default:
                return Vector3.zero;
        }
    }

    // Determinar la direcci�n en la que se mover� el meteorito, dependiendo de su borde de origen
    Vector2 ObtenerDireccionMovimiento(int borde)
    {
        // Definir un �ngulo aleatorio para el movimiento del meteorito
        float angulo = Random.Range(-90f, 90f); // �ngulo en grados, de -45� a 45� (se puede ajustar seg�n necesidad)

        // Convertir el �ngulo en radianes
        float anguloEnRadianes = angulo * Mathf.Deg2Rad;

        // Calcular la direcci�n del meteorito en base al �ngulo
        Vector2 direccion = new Vector2(Mathf.Cos(anguloEnRadianes), Mathf.Sin(anguloEnRadianes));

        return direccion.normalized;  // Devolver la direcci�n normalizada
    }

    // Comprobar si el meteorito est� fuera de la pantalla
    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }
}

