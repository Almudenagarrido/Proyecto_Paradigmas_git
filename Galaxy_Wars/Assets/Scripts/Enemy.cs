using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float minVelocidad;
    public float maxVelocidad;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);  // Elige un borde aleatorio (arriba, abajo, izquierda, derecha)
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);

        // Establecer la posición inicial
        transform.position = puntoDeSalida;

        // Calcular la dirección del meteorito
        direccion = ObtenerDireccionMovimiento(borde);

        RotarHaciaDireccion();
    }

    void Update()
    {
        velocidad = Random.Range(minVelocidad, maxVelocidad);
        transform.Translate(direccion * velocidad * Time.deltaTime);

        RotarHaciaDireccion();

        // Si el meteorito sale de la pantalla, destruirlo
        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    void RotarHaciaDireccion()
    {
        // Calcula el ángulo usando la dirección del movimiento
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Aplica la rotación al transform (el sprite apunta hacia la derecha de forma predeterminada)
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }


    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }

    Vector3 ObtenerPosicionBorde(int borde)
    {
        float bordeExtra = 1f; // Margen adicional fuera de la pantalla

        // Obtener las dimensiones de la cámara
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

    Vector2 ObtenerDireccionMovimiento(int borde)
    {
        // Definir un ángulo aleatorio para el movimiento del meteorito
        float angulo = Random.Range(-90f, 90f); // Ángulo en grados, de -45° a 45° (se puede ajustar según necesidad)

        // Convertir el ángulo en radianes
        float anguloEnRadianes = angulo * Mathf.Deg2Rad;

        // Calcular la dirección del meteorito en base al ángulo
        Vector2 direccion = new Vector2(Mathf.Cos(anguloEnRadianes), Mathf.Sin(anguloEnRadianes));

        return direccion.normalized;  // Devolver la dirección normalizada
    }
}
