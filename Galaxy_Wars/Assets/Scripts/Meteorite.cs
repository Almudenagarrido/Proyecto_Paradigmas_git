using System.Collections;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public float minVelocidad;
    public float maxVelocidad;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;

    public Sprite explosionSprite;
    private float minScale = 0.6f;
    private float maxScale = 1.0f;

    private GameManager gameManager;

    void Start()
    {
        GameObject managerObj = GameObject.Find("GameManager");
        gameManager = managerObj.GetComponent<GameManager>();

        camara = Camera.main;

        float randomScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;
        direccion = ObtenerDireccionMovimiento(borde);
    }

    void Update()
    {
        velocidad = Random.Range(minVelocidad, maxVelocidad);
        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    Vector3 ObtenerPosicionBorde(int borde)
    {
        float bordeExtra = 1f;

        float anchoPantalla = camara.aspect * camara.orthographicSize;
        float altoPantalla = camara.orthographicSize;

        switch (borde)
        {
            case 0: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), altoPantalla + bordeExtra, 0);
            case 1: return new Vector3(Random.Range(-anchoPantalla, anchoPantalla), -altoPantalla - bordeExtra, 0);
            case 2: return new Vector3(-anchoPantalla - bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            case 3: return new Vector3(anchoPantalla + bordeExtra, Random.Range(-altoPantalla, altoPantalla), 0);
            default: return Vector3.zero;
        }
    }

    Vector2 ObtenerDireccionMovimiento(int borde)
    {
        float angulo = Random.Range(-90f, 90f);
        float anguloEnRadianes = angulo * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(anguloEnRadianes), Mathf.Sin(anguloEnRadianes)).normalized;
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }

    private void Explode()
    {
        StartCoroutine(SimplifiedExplosion());
    }

    private IEnumerator SimplifiedExplosion()
    {
        // Crear el humo rojo
        GameObject redSmoke = CreateSmokeObject(explosionSprite, 0.08f, -0.1f);

        float totalDuration = 1f; // Duración total de la explosión
        float maxScale = 0.6f;    // Escala máxima del humo
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / totalDuration;
            UpdateSmokeScale(redSmoke, progress, maxScale);

            yield return null;
        }

        Destroy(redSmoke);
        if (redSmoke != null) { Destroy(redSmoke); }
        if (redSmoke != null) { Destroy(redSmoke); }

        Destroy(gameObject);
    }

    private GameObject CreateSmokeObject(Sprite sprite, float initialScale, float zOffset)
    {
        GameObject smoke = new GameObject("Smoke");
        smoke.transform.position = transform.position + new Vector3(0, 0, zOffset);
        smoke.transform.localScale = new Vector3(initialScale, initialScale, 1);

        SpriteRenderer renderer = smoke.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 10;

        return smoke;
    }

    private void UpdateSmokeScale(GameObject smoke, float progress, float maxScale)
    {
        float scale = Mathf.Lerp(smoke.transform.localScale.x, maxScale, progress);
        smoke.transform.localScale = new Vector3(scale, scale, 1);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {              
            if (gameManager.numberOfPlayers == 1)
            {
                Explode();
                gameManager.AddPoints(1, "Meteorite");
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager.numberOfPlayers == 1)
            {
                gameManager.TakeLife(1, "Meteorite");
            }
            Explode();
        }
    }
}
