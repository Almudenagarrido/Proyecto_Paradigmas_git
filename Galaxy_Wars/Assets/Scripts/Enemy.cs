using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    public float minVelocidad = 1.5f;
    public float maxVelocidad = 3f;
    protected float velocidad;
    protected Camera camara;

    public Sprite explosionSprite;
    protected GameManager gameManager;
    protected Transform player;
    protected int hits;
    protected bool isExploding;

    protected virtual void Start()
    {
        // Referencia al GameManager
        GameObject managerObj = GameObject.Find("GameManager");
        gameManager = managerObj.GetComponent<GameManager>();

        // Configuración de la cámara
        camara = Camera.main;

        // Determinar desde qué borde aparece el enemigo
        int borde = Random.Range(0, 4);
        Vector3 puntoDeSalida = ObtenerPosicionBorde(borde);
        transform.position = puntoDeSalida;

        // Buscar al jugador
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        velocidad = Random.Range(minVelocidad, maxVelocidad);
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            MoverHaciaJugador();
        }

        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    protected void MoverHaciaJugador()
    {
        Vector2 direccion = ((Vector2)player.position - (Vector2)transform.position).normalized;
        transform.Translate(direccion * velocidad * Time.deltaTime);
        RotarHaciaDireccion(direccion);
    }

    protected void RotarHaciaDireccion(Vector2 direccion)
    {
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    protected Vector3 ObtenerPosicionBorde(int borde)
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

    protected bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }

    protected void Explode()
    {
        StartCoroutine(SimplifiedExplosion());
    }

    private IEnumerator SimplifiedExplosion()
    {
        GameObject redSmoke = CreateSmokeObject(explosionSprite, 0.08f, -0.1f);
        Destroy(gameObject);

        float totalDuration = 1f;
        float maxScale = 20f;
        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / totalDuration;
            UpdateSmokeScale(redSmoke, progress, maxScale);
            yield return null;
        }

        Destroy(redSmoke);
        
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

    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
