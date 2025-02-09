using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Camera camara;

    void Start()
    {
        camara = Camera.main;
    }

    void Update()
    {
        // Destruir la bala si sale de los l�mites de la pantalla
        if (!EnPantalla())
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("EnemyNoob"))
        {
            Destroy(gameObject);
        }
    }

    bool EnPantalla()
    {
        Vector3 posicionEnPantalla = camara.WorldToViewportPoint(transform.position);
        return posicionEnPantalla.x > -0.1f && posicionEnPantalla.x < 1.1f &&
               posicionEnPantalla.y > -0.1f && posicionEnPantalla.y < 1.1f;
    }
}

