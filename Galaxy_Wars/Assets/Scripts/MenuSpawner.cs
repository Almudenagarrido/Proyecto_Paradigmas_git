using System.Collections.Generic;
using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    public Sprite meteoritoSprite;
    public List<Sprite> navesSprites;
    public float intervaloGeneracion = 0.7f;
    public Vector2 escalaMinMaxMeteoritos = new Vector2(0.7f, 1f);
    public Vector3 escalaNaves = new Vector3(0.8f, 0.8f, 1f);

    void Start()
    {
        InvokeRepeating(nameof(GenerarObjeto), 0f, intervaloGeneracion); // Genera objetos indefinidamente
    }

    void GenerarObjeto()
    {
        if (Random.value > 0.2f)
        {
            GenerarMeteorito();
        }
        else
        {
            GenerarNave();
        }
    }

    void GenerarMeteorito()
    {
        Vector3 posicion = ObtenerPosicionBordePantalla(out Vector2 direccion);

        // Crear un meteorito utilizando un GameObject vacío
        GameObject meteorito = new GameObject("Meteorito");
        SpriteRenderer renderer = meteorito.AddComponent<SpriteRenderer>();
        renderer.sprite = meteoritoSprite;
        renderer.sortingOrder = 1;

        // Ajustar tamaño aleatorio del meteorito
        float escala = Random.Range(escalaMinMaxMeteoritos.x, escalaMinMaxMeteoritos.y);
        meteorito.transform.localScale = new Vector3(escala, escala, 1f);
        meteorito.transform.position = posicion;

        // Agregar movimiento definido
        Rigidbody2D rb = meteorito.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = direccion * 4f;
    }

    void GenerarNave()
    {
        if (navesSprites.Count == 0) return;

        Vector3 posicion = ObtenerPosicionBordePantalla(out Vector2 direccion);

        // Crear una nave utilizando el GameObject vacío base
        GameObject nave = new GameObject("Nave");
        SpriteRenderer renderer = nave.AddComponent<SpriteRenderer>();
        renderer.sprite = navesSprites[Random.Range(0, navesSprites.Count)];
        renderer.sortingOrder = 1;

        // Escala fija para las naves
        nave.transform.localScale = escalaNaves;
        nave.transform.position = posicion;

        // Calcular ángulo para rotar la nave hacia la dirección de movimiento
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        nave.transform.rotation = Quaternion.Euler(0, 0, angulo - 90);

        // Agregar movimiento definido
        Rigidbody2D rb = nave.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = direccion * Random.Range(4f, 5f); // Velocidad aleatoria
    }

    Vector3 ObtenerPosicionBordePantalla(out Vector2 direccion)
    {
        Camera camara = Camera.main;

        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        // Elegir un borde aleatorio
        int borde = Random.Range(0, 4);
        Vector3 posicion = Vector3.zero;

        switch (borde)
        {
            case 0: // Arriba
                posicion = new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + 2f, 0);
                break;
            case 1: // Abajo
                posicion = new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - 2f, 0);
                break;
            case 2: // Izquierda
                posicion = new Vector3(-anchoPantalla - 2f, Random.Range(-altoPantalla, altoPantalla), 0);
                break;
            case 3: // Derecha
                posicion = new Vector3(anchoPantalla + 2f, Random.Range(-altoPantalla, altoPantalla), 0);
                break;
        }

        // Generar un punto objetivo aleatorio dentro de la pantalla
        Vector3 objetivo = new Vector3(
            Random.Range(-anchoPantalla, anchoPantalla),
            Random.Range(-altoPantalla, altoPantalla),
            0
        );

        // Calcular la dirección hacia el punto objetivo y normalizarla
        direccion = (objetivo - posicion).normalized;

        return posicion;
    }
}
