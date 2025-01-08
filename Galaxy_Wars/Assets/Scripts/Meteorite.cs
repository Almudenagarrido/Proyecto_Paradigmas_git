using System.Collections;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public float minVelocidad;
    public float maxVelocidad;
    private Vector2 direccion;
    private float velocidad;
    private Camera camara;
    private int hits = 0;

    public Sprite[] explosionSprites;
    private float minScale = 0.6f;
    private float maxScale = 1.0f;

    void Start()
    {
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
        StartCoroutine(ExplosionAnimation());
    }

    private IEnumerator ExplosionAnimation()
    {
        GameObject redSmoke = CreateSmokeObject(explosionSprites[2], 0.15f, -0.1f);
        GameObject yellowSmoke = CreateSmokeObject(explosionSprites[1], 0.125f, -0.07f);
        GameObject whiteSmoke = CreateSmokeObject(explosionSprites[0], 0.1f, -0.05f);

        float totalDuration = 2f;
        float redDuration = 1.6f;
        float yellowDuration = 1.8f;
        float whiteDuration = 2f;
        float fadeStartTime = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;

            // Escalado progresivo
            if (elapsedTime < redDuration)
            {
                UpdateSmokeScale(redSmoke, elapsedTime / redDuration, 0.45f);
            }
            if (elapsedTime < yellowDuration)
            {
                UpdateSmokeScale(yellowSmoke, elapsedTime / yellowDuration, 0.5f);
            }
            UpdateSmokeScale(whiteSmoke, elapsedTime / whiteDuration, 0.75f);

            // Transparencia progresiva, comienza a los 2.5 segundos
            if (elapsedTime > fadeStartTime && elapsedTime <= redDuration)
            {
                UpdateSmokeTransparency(redSmoke, 1f - (elapsedTime - fadeStartTime) / (redDuration - fadeStartTime));
            }
            if (elapsedTime > fadeStartTime && elapsedTime <= yellowDuration)
            {
                UpdateSmokeTransparency(yellowSmoke, 1f - (elapsedTime - fadeStartTime) / (yellowDuration - fadeStartTime));
            }
            if (elapsedTime > fadeStartTime && elapsedTime <= whiteDuration)
            {
                UpdateSmokeTransparency(whiteSmoke, 1f - (elapsedTime - fadeStartTime) / (whiteDuration - fadeStartTime));
            }

            yield return null;
        }

        Destroy(redSmoke);
        Destroy(yellowSmoke);
        Destroy(whiteSmoke);

        GameManager.Instance.CheckGameOver();
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
        renderer.color = new Color(1f, 1f, 1f, 1f);

        return smoke;
    }

    private void UpdateSmokeScale(GameObject smoke, float progress, float maxScale)
    {
        float scale = Mathf.Lerp(smoke.transform.localScale.x, maxScale, progress);
        smoke.transform.localScale = new Vector3(scale, scale, 1);
    }

    private void UpdateSmokeTransparency(GameObject smoke, float alpha)
    {
        SpriteRenderer renderer = smoke.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            hits++;
            if (hits >= 2)
            {
                Explode();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }
}
