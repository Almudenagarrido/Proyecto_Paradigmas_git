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

    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float bulletSpeed = 7f;
    public float timeBetweenBullets = 0.15f;
    private float lastShot = 0f;

    void Start()
    {
        camara = Camera.main;

        // Determinar desde qu� borde aparece el enemigo
        int borde = Random.Range(0, 4);  // Elige un borde aleatorio (arriba, abajo, izquierda, derecha)
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);

        // Establecer la posici�n inicial
        transform.position = puntoDeSalida;

        // Calcular la direcci�n del meteorito
        direccion = ObtenerDireccionMovimiento(borde);

        RotarHaciaDireccion();

        // Disparar
        InvokeRepeating("Shoot", 1f, timeBetweenBullets);

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
        // Calcula el �ngulo usando la direcci�n del movimiento
        //float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Aplica la rotaci�n al transform (el sprite apunta hacia la derecha de forma predeterminada)
        //transform.rotation = Quaternion.Euler(0, 0, angulo);
        transform.up = direccion;
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

    void Shoot()
    {
        // Instancia la bala en el punto de disparo
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Rotar la bala hacia la direcci�n del enemigo
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Configurar la velocidad de la bala
        Rigidbody2D rigidBullet = bullet.GetComponent<Rigidbody2D>();
        rigidBullet.freezeRotation = true;
        rigidBullet.velocity = shootingPoint.up * bulletSpeed;

        // Desactivar colisi�n entre la bala y el enemigo
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
